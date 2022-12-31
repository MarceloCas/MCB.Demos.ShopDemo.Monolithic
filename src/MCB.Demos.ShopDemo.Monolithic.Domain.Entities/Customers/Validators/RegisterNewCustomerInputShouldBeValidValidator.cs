using MCB.Core.Domain.Entities.DomainEntitiesBase.Validators;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Validators.Wrappers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Validators.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Validators;

public sealed class RegisterNewCustomerInputShouldBeValidValidator
    : InputBaseValidator<RegisterNewCustomerInput>,
    IRegisterNewCustomerInputShouldBeValidValidator
{
    // Fields
    private readonly ICustomerSpecifications _customerSpecifications;

    // Constructors
    public RegisterNewCustomerInputShouldBeValidValidator(
        IInputBaseSpecifications inputBaseSpecifications,
        IDateTimeProvider dateTimeProvider
    ): base(inputBaseSpecifications)
    {
        _customerSpecifications = new CustomerSpecifications(dateTimeProvider);
    }

    // Protected Methods
    protected override void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        // FirstName
        CustomerValidatorWrapper.AddCustomerShouldHaveFirstName(
            _customerSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.FirstName,
            getFirstNameFunction: input => input.FirstName
        );
        CustomerValidatorWrapper.AddCustomerShouldHaveFirstNameMaximumLength(
            _customerSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.FirstName,
            getFirstNameFunction: input => input.FirstName
        );

        // LastName
        CustomerValidatorWrapper.AddCustomerShouldHaveLastName(
            _customerSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.LastName,
            getLastNameFunction: input => input.LastName
        );
        CustomerValidatorWrapper.AddCustomerShouldHaveLastNameMaximumLength(
            _customerSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.LastName,
            getLastNameFunction: input => input.LastName
        );

        // BirthDate
        CustomerValidatorWrapper.AddCustomerShouldHaveBirthDate(
            _customerSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.BirthDate,
            getBirthDateFunction: input => input.BirthDate
        );
        CustomerValidatorWrapper.AddCustomerShouldHaveValidBirthDate(
            _customerSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.BirthDate,
            getBirthDateFunction: input => input.BirthDate
        );
    }
}