using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders;
public class Order
    : DomainEntityBase,
    IAggregationRoot
{
    // Properties
    public string Code { get; private set; }
    public DateTime Date { get; private set; }
    public Customers.Customer Customer { get; private set; } = null!;

    // Constructors
    public Order(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
        Code = string.Empty;
    }

    // Protected Methods
    protected override DomainEntityBase CreateInstanceForCloneInternal() => new Order(DateTimeProvider);
}
