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
    public static readonly string OrderItemShouldHaveSequenceMessage = nameof(OrderItemShouldHaveSequenceMessage);
    public static readonly Severity OrderItemShouldHaveSequenceSeverity = Severity.Error;

    // Description
    public static readonly string OrderItemShouldHaveDescriptionMaximumLengthErrorCode = nameof(OrderItemShouldHaveDescriptionMaximumLengthErrorCode);
    public static readonly string OrderItemShouldHaveDescriptionMaximumLengthMessage = nameof(OrderItemShouldHaveDescriptionMaximumLengthMessage);
    public static readonly Severity OrderItemShouldHaveDescriptionMaximumLengthSeverity = Severity.Error;

    // Quantity
    public static readonly string OrderItemShouldHaveQuantityErrorCode = nameof(OrderItemShouldHaveQuantityErrorCode);
    public static readonly string OrderItemShouldHaveQuantityMessage = nameof(OrderItemShouldHaveQuantityMessage);
    public static readonly Severity OrderItemShouldHaveQuantitySeverity = Severity.Error;

    // UnityValue
    public static readonly string OrderItemShouldHaveUnityValueErrorCode = nameof(OrderItemShouldHaveUnityValueErrorCode);
    public static readonly string OrderItemShouldHaveUnityValueMessage = nameof(OrderItemShouldHaveUnityValueMessage);
    public static readonly Severity OrderItemShouldHaveUnityValueSeverity = Severity.Error;

    // Product
    public static readonly string OrderItemShouldHaveProductErrorCode = nameof(OrderItemShouldHaveProductErrorCode);
    public static readonly string OrderItemShouldHaveProductMessage = nameof(OrderItemShouldHaveProductMessage);
    public static readonly Severity OrderItemShouldHaveProductSeverity = Severity.Error;

    // Methods
    bool OrderItemShouldHaveSequence(int sequence);
    bool OrderItemShouldHaveDescriptionMaximumLength(string? description);
    bool OrderItemShouldHaveQuantity(decimal quantity);
    bool OrderItemShouldHaveUnityValue(decimal unityValue);
    bool OrderItemShouldHaveProduct(Product? product);
}
