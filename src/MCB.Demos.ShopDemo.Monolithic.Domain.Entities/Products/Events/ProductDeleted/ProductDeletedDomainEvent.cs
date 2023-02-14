using MCB.Core.Domain.Entities.DomainEntitiesBase.Events;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductDeleted;
public record ProductDeletedDomainEvent
    : DomainEventBase
{
    public ProductDeletedDomainEvent(
        Guid correlationId,
        Guid id,
        Guid tenantId,
        DateTime timestamp,
        string executionUser,
        string sourcePlatform,
        string domainEventType,
        Product product
    ) : base(correlationId, id, tenantId, timestamp, executionUser, sourcePlatform, domainEventType, product)
    {
    }
}