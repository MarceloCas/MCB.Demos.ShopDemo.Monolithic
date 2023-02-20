using FluentValidation;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using System.Linq.Expressions;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Wrappers;
public static class OrderItemValidatorWrapper
{
    // Sequence
    public static void AddOrderItemShouldHaveSequence<TInput, TProperty>(
        IOrderItemSpecifications orderItemSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, int> getSequenceFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderItemSpecifications.OrderItemShouldHaveSequence(getSequenceFunction(input)))
            .WithErrorCode(IOrderItemSpecifications.OrderItemShouldHaveSequenceErrorCode)
            .WithMessage(IOrderItemSpecifications.OrderItemShouldHaveSequenceMessage)
            .WithSeverity(IOrderItemSpecifications.OrderItemShouldHaveSequenceSeverity);
    }

    // Description
    public static void AddOrderItemShouldHaveDescriptionMaximumLength<TInput, TProperty>(
        IOrderItemSpecifications orderItemSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string?> getDescriptionFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, sequence) => orderItemSpecifications.OrderItemShouldHaveDescriptionMaximumLength(getDescriptionFunction(input)))
            .When(input => !string.IsNullOrEmpty(getDescriptionFunction(input)))
            .WithErrorCode(IOrderItemSpecifications.OrderItemShouldHaveDescriptionMaximumLengthErrorCode)
            .WithMessage(IOrderItemSpecifications.OrderItemShouldHaveDescriptionMaximumLengthMessage)
            .WithSeverity(IOrderItemSpecifications.OrderItemShouldHaveDescriptionMaximumLengthSeverity);
    }

    // Quantity
    public static void AddOrderItemShouldHaveQuantity<TInput, TProperty>(
        IOrderItemSpecifications orderItemSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, decimal> getQuantityFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, quantity) => orderItemSpecifications.OrderItemShouldHaveQuantity(getQuantityFunction(input)))
            .WithErrorCode(IOrderItemSpecifications.OrderItemShouldHaveQuantityErrorCode)
            .WithMessage(IOrderItemSpecifications.OrderItemShouldHaveQuantityMessage)
            .WithSeverity(IOrderItemSpecifications.OrderItemShouldHaveQuantitySeverity);
    }

    // UnityValue
    public static void AddOrderItemShouldHaveUnityValue<TInput, TProperty>(
        IOrderItemSpecifications orderItemSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, decimal> getUnityValueFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, unityValue) => orderItemSpecifications.OrderItemShouldHaveUnityValue(getUnityValueFunction(input)))
            .WithErrorCode(IOrderItemSpecifications.OrderItemShouldHaveUnityValueErrorCode)
            .WithMessage(IOrderItemSpecifications.OrderItemShouldHaveUnityValueMessage)
            .WithSeverity(IOrderItemSpecifications.OrderItemShouldHaveUnityValueSeverity);
    }

    // Product
    public static void AddOrderItemShouldHaveProduct<TInput, TProperty>(
        IOrderItemSpecifications orderItemSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, Product> getProductFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, unityValue) => orderItemSpecifications.OrderItemShouldHaveProduct(getProductFunction(input)))
            .WithErrorCode(IOrderItemSpecifications.OrderItemShouldHaveProductErrorCode)
            .WithMessage(IOrderItemSpecifications.OrderItemShouldHaveProductMessage)
            .WithSeverity(IOrderItemSpecifications.OrderItemShouldHaveProductSeverity);
    }
}
