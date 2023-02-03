using MCB.Core.Domain.Entities.DomainEntitiesBase.Events;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted;
public record CustomerDeletedDomainEvent
    : DomainEventBase
{
    public CustomerDeletedDomainEvent(
        Guid correlationId,
        Guid id,
        Guid tenantId,
        DateTime timestamp,
        string executionUser,
        string sourcePlatform,
        string domainEventType,
        Customer customer
    ) : base(correlationId, id, tenantId, timestamp, executionUser, sourcePlatform, domainEventType, customer)
    {
    }
}
