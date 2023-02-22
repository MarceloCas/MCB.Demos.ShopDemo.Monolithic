using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderRemoved.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderRemoved.Factories;
public class OrderRemovedDomainEventFactory
    : DomainEventFactoryBase,
    IOrderRemovedDomainEventFactory
{
    // Constructors
    public OrderRemovedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public OrderRemovedDomainEvent Create((Order Order, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<OrderRemovedDomainEvent>();

        return new OrderRemovedDomainEvent(
            correlationId: parameter.CorrelationId,
            id: id,
            tenantId: parameter.Order.TenantId,
            timestamp: timestamp,
            executionUser: parameter.ExecutionUser,
            sourcePlatform: parameter.SourcePlatform,
            domainEventType: domainEventType,
            parameter.Order
        );
    }
}