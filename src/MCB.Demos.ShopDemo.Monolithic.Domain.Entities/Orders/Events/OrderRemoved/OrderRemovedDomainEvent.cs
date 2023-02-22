using MCB.Core.Domain.Entities.DomainEntitiesBase.Events;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderRemoved;
public record OrderRemovedDomainEvent
    : DomainEventBase
{
    public OrderRemovedDomainEvent(
        Guid correlationId,
        Guid id,
        Guid tenantId,
        DateTime timestamp,
        string executionUser,
        string sourcePlatform,
        string domainEventType,
        Order order
    ) : base(correlationId, id, tenantId, timestamp, executionUser, sourcePlatform, domainEventType, order)
    {
    }
}
