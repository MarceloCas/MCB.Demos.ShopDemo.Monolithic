using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Specifications.Interfaces;

public interface IProductSpecifications
    : IDomainEntitySpecifications
{
    // Constants
    public const int PRODUCT_CODE_MAX_LENGTH = 150;
    public const int PRODUCT_DESCRIPTION_MAX_LENGTH = 250;

    // Code
    public static readonly string ProductShouldHaveCodeErrorCode = nameof(ProductShouldHaveCodeErrorCode);
    public static readonly string ProductShouldHaveCodeErrorMessage = nameof(ProductShouldHaveCodeErrorMessage);
    public static readonly Severity ProductShouldHaveCodeErrorSeverity = Severity.Error;

    public static readonly string ProductShouldHaveCodeMaximumLengthErrorCode = nameof(ProductShouldHaveCodeMaximumLengthErrorCode);
    public static readonly string ProductShouldHaveCodeMaximumLengthErrorMessage = nameof(ProductShouldHaveCodeMaximumLengthErrorMessage);
    public static readonly Severity ProductShouldHaveCodeMaximumLengthErrorSeverity = Severity.Error;

    // Description
    public static readonly string ProductShouldHaveDescriptionErrorCode = nameof(ProductShouldHaveDescriptionErrorCode);
    public static readonly string ProductShouldHaveDescriptionErrorMessage = nameof(ProductShouldHaveDescriptionErrorMessage);
    public static readonly Severity ProductShouldHaveDescriptionErrorSeverity = Severity.Error;

    public static readonly string ProductShouldHaveDescriptionMaximumLengthErrorCode = nameof(ProductShouldHaveDescriptionMaximumLengthErrorCode);
    public static readonly string ProductShouldHaveDescriptionMaximumLengthErrorMessage = nameof(ProductShouldHaveDescriptionMaximumLengthErrorMessage);
    public static readonly Severity ProductShouldHaveDescriptionMaximumLengthErrorSeverity = Severity.Error;

    // Methods
    bool ProductShouldHaveCode(string firstName);
    bool ProductShouldHaveCodeMaximumLength(string firstName);

    bool ProductShouldHaveDescription(string lastName);
    bool ProductShouldHaveDescriptionMaximumLength(string lastName);
}
