using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories;
public class OrderFactory
    : IOrderFactory
{
    // Fields
    private readonly IDateTimeProvider _dateTimeProvider;

    // Constructors
    public OrderFactory(
        IDateTimeProvider dateTimeProvider
    )
    {
        _dateTimeProvider = dateTimeProvider;
    }

    // Public Methods
    public Order? Create()
    {
        return new Order(_dateTimeProvider);
    }
}
