using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Specifications;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Validators.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Validators.Wrappers;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Validators;

public class EmailValueObjectShouldBeValidValidator
    : ValidatorBase<EmailValueObject>,
    IEmailValueObjectShouldBeValidValidator
{
    // Fields
    private readonly IEmailValueObjectSpecifications _emailValueObjectSpecifications;

    // Constructors
    public EmailValueObjectShouldBeValidValidator()
    {
        _emailValueObjectSpecifications = new EmailValueObjectSpecifications();
    }

    // Protected Methods
    protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        EmailValueObjectValidatorWrapper.AddEmailValueObjectShouldRequired(
            _emailValueObjectSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: q => q,
            getEmailFunction: q => q
        );
        EmailValueObjectValidatorWrapper.AddEmailValueObjectShouldHaveMaximumLength(
            _emailValueObjectSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: q => q,
            getEmailFunction: q => q
        );
    }
}