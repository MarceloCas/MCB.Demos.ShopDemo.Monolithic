using MCB.Core.Domain.Entities.DomainEntitiesBase.Events;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported;
public record ProductImportedDomainEvent
    : DomainEventBase
{
    public ProductImportedDomainEvent(
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