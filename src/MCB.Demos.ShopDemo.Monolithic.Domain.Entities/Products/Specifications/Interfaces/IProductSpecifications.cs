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
    public static readonly string ProductShouldHaveCodeMessage = nameof(ProductShouldHaveCodeMessage);
    public static readonly Severity ProductShouldHaveCodeSeverity = Severity.Error;

    public static readonly string ProductShouldHaveCodeMaximumLengthErrorCode = nameof(ProductShouldHaveCodeMaximumLengthErrorCode);
    public static readonly string ProductShouldHaveCodeMaximumLengthMessage = nameof(ProductShouldHaveCodeMaximumLengthMessage);
    public static readonly Severity ProductShouldHaveCodeMaximumLengthSeverity = Severity.Error;

    // Description
    public static readonly string ProductShouldHaveDescriptionErrorCode = nameof(ProductShouldHaveDescriptionErrorCode);
    public static readonly string ProductShouldHaveDescriptionMessage = nameof(ProductShouldHaveDescriptionMessage);
    public static readonly Severity ProductShouldHaveDescriptionSeverity = Severity.Error;

    public static readonly string ProductShouldHaveDescriptionMaximumLengthErrorCode = nameof(ProductShouldHaveDescriptionMaximumLengthErrorCode);
    public static readonly string ProductShouldHaveDescriptionMaximumLengthMessage = nameof(ProductShouldHaveDescriptionMaximumLengthMessage);
    public static readonly Severity ProductShouldHaveDescriptionMaximumLengthSeverity = Severity.Error;

    // Methods
    bool ProductShouldHaveCode(string firstName);
    bool ProductShouldHaveCodeMaximumLength(string firstName);

    bool ProductShouldHaveDescription(string lastName);
    bool ProductShouldHaveDescriptionMaximumLength(string lastName);
}
