﻿using FluentValidation;
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
            .WithMessage(IOrderSpecifications.OrderShouldHaveCodeErrorMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveCodeErrorSeverity);
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
            .WithMessage(IOrderSpecifications.OrderShouldHaveDateErrorMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveDateErrorSeverity);
    }
    public static void AddOrderShouldHaveDateWithValidLength<TInput, TProperty>(
        IOrderSpecifications orderSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, DateTime?> getDateFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderSpecifications.OrderShouldHaveDateWithValidLength(getDateFunction(input)))
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveDateWithValidLengthErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveDateWithValidLengthErrorMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveDateWithValidLengthErrorSeverity);
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
            .WithMessage(IOrderSpecifications.OrderShouldHaveCustomerErrorMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveCustomerErrorSeverity);
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
            .When(input => input != null)
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveOrderItemsErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveOrderItemsErrorMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveOrderItemsErrorSeverity);
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
            .When(input => input != null)
            .WithErrorCode(IOrderSpecifications.OrderShouldHaveOrderItemsErrorCode)
            .WithMessage(IOrderSpecifications.OrderShouldHaveOrderItemsErrorMessage)
            .WithSeverity(IOrderSpecifications.OrderShouldHaveOrderItemsErrorSeverity);
    }
}
