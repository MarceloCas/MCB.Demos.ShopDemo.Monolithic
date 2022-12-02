using FluentValidation;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications.Interfaces;
using System.Linq.Expressions;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Validators.Wrappers;

public static class CustomerValidatorWrapper
{
    // First Name
    public static void AddCustomerShouldHaveFirstName<TInput, TProperty>(
        ICustomerSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getFirstNameFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, firstName) => customerSpecifications.CustomerShouldHaveFirstName(getFirstNameFunction(input)))
            .WithErrorCode(ICustomerSpecifications.CustomerShouldHaveFirstNameErrorCode)
            .WithMessage(ICustomerSpecifications.CustomerShouldHaveFirstNameMessage)
            .WithSeverity(ICustomerSpecifications.CustomerShouldHaveFirstNameSeverity);
    }
    public static void AddCustomerShouldHaveFirstNameMaximumLength<TInput, TProperty>(
        ICustomerSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getFirstNameFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, firstName) => customerSpecifications.CustomerShouldHaveFirstNameMaximumLength(getFirstNameFunction(input)))
            .When(input => customerSpecifications.CustomerShouldHaveFirstName(getFirstNameFunction(input)))
            .WithErrorCode(ICustomerSpecifications.CustomerShouldHaveFirstNameMaximumLengthErrorCode)
            .WithMessage(ICustomerSpecifications.CustomerShouldHaveFirstNameMaximumLengthMessage)
            .WithSeverity(ICustomerSpecifications.CustomerShouldHaveFirstNameMaximumLengthSeverity);
    }

    // LastName
    public static void AddCustomerShouldHaveLastName<TInput, TProperty>(
        ICustomerSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getLastNameFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, lastName) => customerSpecifications.CustomerShouldHaveLastName(getLastNameFunction(input)))
            .WithErrorCode(ICustomerSpecifications.CustomerShouldHaveLastNameErrorCode)
            .WithMessage(ICustomerSpecifications.CustomerShouldHaveLastNameMessage)
            .WithSeverity(ICustomerSpecifications.CustomerShouldHaveLastNameSeverity);
    }
    public static void AddCustomerShouldHaveLastNameMaximumLength<TInput, TProperty>(
        ICustomerSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getLastNameFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, lastName) => customerSpecifications.CustomerShouldHaveLastNameMaximumLength(getLastNameFunction(input)))
            .When(input => customerSpecifications.CustomerShouldHaveLastName(getLastNameFunction(input)))
            .WithErrorCode(ICustomerSpecifications.CustomerShouldHaveLastNameMaximumLengthErrorCode)
            .WithMessage(ICustomerSpecifications.CustomerShouldHaveLastNameMaximumLengthMessage)
            .WithSeverity(ICustomerSpecifications.CustomerShouldHaveLastNameMaximumLengthSeverity);
    }

    // BirthDate
    public static void AddCustomerShouldHaveBirthDate<TInput, TProperty>(
        ICustomerSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, DateTime> getBirthDateFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, birthDate) => customerSpecifications.CustomerShouldHaveBirthDate(getBirthDateFunction(input)))
            .WithErrorCode(ICustomerSpecifications.CustomerShouldHaveBirthDateErrorCode)
            .WithMessage(ICustomerSpecifications.CustomerShouldHaveBirthDateMessage)
            .WithSeverity(ICustomerSpecifications.CustomerShouldHaveBirthDateSeverity);
    }
    public static void AddCustomerShouldHaveValidBirthDate<TInput, TProperty>(
        ICustomerSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, DateTime> getBirthDateFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, birthDate) => customerSpecifications.CustomerShouldHaveValidBirthDate(getBirthDateFunction(input)))
            .When(input => customerSpecifications.CustomerShouldHaveBirthDate(getBirthDateFunction(input)))
            .WithErrorCode(ICustomerSpecifications.CustomerShouldHaveValidBirthDateErrorCode)
            .WithMessage(ICustomerSpecifications.CustomerShouldHaveValidBirthDateMessage)
            .WithSeverity(ICustomerSpecifications.CustomerShouldHaveValidBirthDateSeverity);
    }
}