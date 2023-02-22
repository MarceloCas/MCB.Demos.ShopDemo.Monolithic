using FluentValidation;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
using System.Linq.Expressions;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Wrappers;
public static class OrderValidatorWrapper
{
    // Code
    public static void AddOrderShouldHaveCode<TInput, TProperty>(
        IOrderSpecifications orderSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string?> getCodeFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderSpecifications.OrderShouldHaveCode(getCodeFunction(input)))
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveCodeErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveCodeMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveCodeSeverity);
    }

    // Date
    public static void AddOrderShouldHaveDate<TInput, TProperty>(
        IOrderSpecifications orderSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, DateTime?> getDateFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderSpecifications.OrderShouldHaveDate(getDateFunction(input)))
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveDateErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveDateMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveDateSeverity);
    }
    public static void AddOrderShouldHaveDateWithValidLength<TInput, TProperty>(
        IOrderSpecifications orderSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, DateTime?> getDateWithValidLengthFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderSpecifications.OrderShouldHaveDateWithValidLength(getDateWithValidLengthFunction(input)))
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveDateWithValidLengthErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveDateWithValidLengthMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveDateWithValidLengthSeverity);
    }

    // Customer
    public static void AddOrderShouldHaveCustomer<TInput, TProperty>(
        IOrderSpecifications orderSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, Customer?> getCustomerFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderSpecifications.OrderShouldHaveCustomer(getCustomerFunction(input)))
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveCustomerErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveCustomerMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveCustomerSeverity);
    }

    // OrderItems
    public static void AddOrderShouldHaveOrderItems<TInput, TProperty>(
        IOrderSpecifications orderSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, ImportOrderInput> getImportOrderInputFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderSpecifications.OrderShouldHaveOrderItems(getImportOrderInputFunction(input)))
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveOrderItemsErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveOrderItemsMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveOrderItemsSeverity);
    }
    public static void AddOrderShouldHaveOrderItemsWithValidSequence<TInput, TProperty>(
        IOrderSpecifications orderSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, ImportOrderInput> getImportOrderInputFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderSpecifications.OrderShouldHaveOrderItemsWithValidSequence(getImportOrderInputFunction(input)))
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveOrderItemsErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveOrderItemsMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveOrderItemsSeverity);
    }
}
