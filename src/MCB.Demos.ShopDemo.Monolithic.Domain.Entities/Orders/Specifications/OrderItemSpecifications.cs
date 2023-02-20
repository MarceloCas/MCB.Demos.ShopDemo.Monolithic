using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications;
public class OrderItemSpecifications
    : DomainEntitySpecifications,
    IOrderItemSpecifications
{
    // Constructors
    public OrderItemSpecifications(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public bool OrderItemShouldHaveSequence(int sequence)
    {
        return sequence > 0;
    }
    public bool OrderItemShouldHaveDescriptionMaximumLength(string? description)
    {
        return description?.Length <= IOrderItemSpecifications.ORDER_ITEM_DESCRIPTION_MAX_LENGTH;
    }
    public bool OrderItemShouldHaveQuantity(decimal quantity)
    {
        return quantity > 0;
    }
    public bool OrderItemShouldHaveUnityValue(decimal unityValue)
    {
        return unityValue > 0;
    }
    public bool OrderItemShouldHaveProduct(Product? product)
    {
        return product != null;
    }
}
