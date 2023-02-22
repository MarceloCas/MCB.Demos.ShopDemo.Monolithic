using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
public interface IOrderItemSpecifications
    : IDomainEntitySpecifications
{
    // Constants
    public const int ORDER_ITEM_DESCRIPTION_MAX_LENGTH = 250;

    // Sequence
    public static readonly string OrderItemShouldHaveSequenceErrorCode = nameof(OrderItemShouldHaveSequenceErrorCode);
    public static readonly string OrderItemShouldHaveSequenceErrorMessage = nameof(OrderItemShouldHaveSequenceErrorMessage);
    public static readonly Severity OrderItemShouldHaveSequenceErrorSeverity = Severity.Error;

    // Description
    public static readonly string OrderItemShouldHaveDescriptionMaximumLengthErrorCode = nameof(OrderItemShouldHaveDescriptionMaximumLengthErrorCode);
    public static readonly string OrderItemShouldHaveDescriptionMaximumLengthErrorMessage = nameof(OrderItemShouldHaveDescriptionMaximumLengthErrorMessage);
    public static readonly Severity OrderItemShouldHaveDescriptionMaximumLengthErrorSeverity = Severity.Error;

    // Quantity
    public static readonly string OrderItemShouldHaveQuantityErrorCode = nameof(OrderItemShouldHaveQuantityErrorCode);
    public static readonly string OrderItemShouldHaveQuantityErrorMessage = nameof(OrderItemShouldHaveQuantityErrorMessage);
    public static readonly Severity OrderItemShouldHaveQuantityErrorSeverity = Severity.Error;

    // UnityValue
    public static readonly string OrderItemShouldHaveUnityValueErrorCode = nameof(OrderItemShouldHaveUnityValueErrorCode);
    public static readonly string OrderItemShouldHaveUnityValueErrorMessage = nameof(OrderItemShouldHaveUnityValueErrorMessage);
    public static readonly Severity OrderItemShouldHaveUnityValueErrorSeverity = Severity.Error;

    // Product
    public static readonly string OrderItemShouldHaveProductErrorCode = nameof(OrderItemShouldHaveProductErrorCode);
    public static readonly string OrderItemShouldHaveProductErrorMessage = nameof(OrderItemShouldHaveProductErrorMessage);
    public static readonly Severity OrderItemShouldHaveProductErrorSeverity = Severity.Error;

    // Methods
    bool OrderItemShouldHaveSequence(int sequence);
    bool OrderItemShouldHaveDescriptionMaximumLength(string? description);
    bool OrderItemShouldHaveQuantity(decimal quantity);
    bool OrderItemShouldHaveUnityValue(decimal unityValue);
    bool OrderItemShouldHaveProduct(Product? product);
}
