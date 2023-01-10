using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Validators;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Validators;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;

public class Customer
    : DomainEntityBase,
    IAggregationRoot
{
    // Properties
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public EmailValueObject Email { get; private set; }
    public int Age
    {
        get
        {
            var currentDate = DateTimeProvider.GetDate();
            var age = currentDate.Year - BirthDate.Year;

            if (currentDate.Month < BirthDate.Month || (currentDate.Month == BirthDate.Month && currentDate.Day < BirthDate.Day))
                age--;

            return age;
        }
    }

    // Validators
    private readonly RegisterNewCustomerInputShouldBeValidValidator _registerNewCustomerInputShouldBeValidValidator;
    private readonly EmailValueObjectShouldBeValidValidator _emailValueObjectShouldBeValidValidator;

    // Constructors
    public Customer(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
        FirstName = string.Empty;
        LastName = string.Empty;

        _registerNewCustomerInputShouldBeValidValidator = new RegisterNewCustomerInputShouldBeValidValidator(
            new InputBaseSpecifications(), 
            dateTimeProvider
        );
        _emailValueObjectShouldBeValidValidator = new EmailValueObjectShouldBeValidValidator();
    }

    // Public Methods
    public Customer RegisterNewCustomer(RegisterNewCustomerInput input)
    {
        // Validate
        if (!Validate(() => _registerNewCustomerInputShouldBeValidValidator.Validate(input)) ||
            !Validate(() => _emailValueObjectShouldBeValidValidator.Validate(input.Email)))
            return this;

        // Process and Return
        return SetName(input.FirstName, input.LastName)
            .SetBirthDate(input.BirthDate)
            .SetEmail(input.Email)
            .RegisterNewInternal<Customer>(input.TenantId, input.ExecutionUser, input.SourcePlatform, input.CorrelationId);
    }
    public Customer SetExistingCustomerInfo(SetExistingCustomerInfoInput input)
    {
        SetExistingInfoInternal<Customer>(
            input.Id,
            input.TenantId,
            input.CreatedBy,
            input.CreatedAt,
            input.LastUpdatedBy,
            input.LastUpdatedAt,
            input.LastSourcePlatform,
            input.RegistryVersion,
            input.CorrelationId
        );

        SetName(input.FirstName, input.LastName);
        SetBirthDate(input.BirthDate);
        SetEmail(input.Email);

        return this;
    }

    public Customer DeepClone()
    {
        // Process and Return
        return DeepCloneInternal<Customer>()
            .SetName(FirstName, LastName)
            .SetBirthDate(BirthDate);
    }

    // Protected Abstract Methods
    protected override DomainEntityBase CreateInstanceForCloneInternal() => new Customer(DateTimeProvider);

    // Private Methods
    private Customer SetName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        return this;
    }
    private Customer SetBirthDate(DateTime birthDate)
    {
        BirthDate = birthDate;

        return this;
    }
    private Customer SetEmail(string email)
    {
        Email = email;
        return this;
    }
}
