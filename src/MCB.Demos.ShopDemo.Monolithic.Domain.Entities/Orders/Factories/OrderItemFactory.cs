using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories;
public class OrderItemFactory
    : IOrderItemFactory
{
    // Fields
    private readonly IDateTimeProvider _dateTimeProvider;

    // Constructors
    public OrderItemFactory(
        IDateTimeProvider dateTimeProvider
    )
    {
        _dateTimeProvider = dateTimeProvider;
    }

    // Public Methods
    public OrderItem? Create()
    {
        return new OrderItem(_dateTimeProvider);
    }
}
