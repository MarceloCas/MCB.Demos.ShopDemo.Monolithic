﻿using FluentValidation;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Specifications.Interfaces;
using System.Linq.Expressions;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Validators.Wrappers;
public static class ProductValidatorWrapper
{
    // Code
    public static void AddProductShouldHaveCode<TInput, TProperty>(
        IProductSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getCodeFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, firstName) => customerSpecifications.ProductShouldHaveCode(getCodeFunction(input)))
            .WithErrorCode(IProductSpecifications.ProductShouldHaveCodeErrorCode)
            .WithMessage(IProductSpecifications.ProductShouldHaveCodeErrorMessage)
            .WithSeverity(IProductSpecifications.ProductShouldHaveCodeErrorSeverity);
    }
    public static void AddProductShouldHaveCodeMaximumLength<TInput, TProperty>(
        IProductSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getCodeFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, firstName) => customerSpecifications.ProductShouldHaveCodeMaximumLength(getCodeFunction(input)))
            .When(input => customerSpecifications.ProductShouldHaveCode(getCodeFunction(input)))
            .WithErrorCode(IProductSpecifications.ProductShouldHaveCodeMaximumLengthErrorCode)
            .WithMessage(IProductSpecifications.ProductShouldHaveCodeMaximumLengthErrorMessage)
            .WithSeverity(IProductSpecifications.ProductShouldHaveCodeMaximumLengthErrorSeverity);
    }

    // Description
    public static void AddProductShouldHaveDescription<TInput, TProperty>(
        IProductSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getDescriptionFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, lastName) => customerSpecifications.ProductShouldHaveDescription(getDescriptionFunction(input)))
            .WithErrorCode(IProductSpecifications.ProductShouldHaveDescriptionErrorCode)
            .WithMessage(IProductSpecifications.ProductShouldHaveDescriptionErrorMessage)
            .WithSeverity(IProductSpecifications.ProductShouldHaveDescriptionErrorSeverity);
    }
    public static void AddProductShouldHaveDescriptionMaximumLength<TInput, TProperty>(
        IProductSpecifications customerSpecifications,
        ValidatorBase<TInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper,
        Expression<Func<TInput, TProperty>> propertyExpression,
        Func<TInput, string> getDescriptionFunction
    )
    {
        fluentValidationValidatorWrapper.RuleFor(propertyExpression)
            .Must((input, lastName) => customerSpecifications.ProductShouldHaveDescriptionMaximumLength(getDescriptionFunction(input)))
            .When(input => customerSpecifications.ProductShouldHaveDescription(getDescriptionFunction(input)))
            .WithErrorCode(IProductSpecifications.ProductShouldHaveDescriptionMaximumLengthErrorCode)
            .WithMessage(IProductSpecifications.ProductShouldHaveDescriptionMaximumLengthErrorMessage)
            .WithSeverity(IProductSpecifications.ProductShouldHaveDescriptionMaximumLengthErrorSeverity);
    }
}
