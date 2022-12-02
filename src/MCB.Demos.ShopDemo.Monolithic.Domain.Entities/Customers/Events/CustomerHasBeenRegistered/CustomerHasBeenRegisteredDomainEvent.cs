using MCB.Core.Domain.Entities.DomainEntitiesBase.Events;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered;

public record CustomerHasBeenRegisteredDomainEvent
    : DomainEventBase
{
    public CustomerHasBeenRegisteredDomainEvent(
        Guid id,
        Guid tenantId,
        DateTime timestamp,
        string executionUser,
        string sourcePlatform,
        string domainEventType,
        Customer customer
    ) : base(id, tenantId, timestamp, executionUser, sourcePlatform, domainEventType, customer)
    {
    }
}