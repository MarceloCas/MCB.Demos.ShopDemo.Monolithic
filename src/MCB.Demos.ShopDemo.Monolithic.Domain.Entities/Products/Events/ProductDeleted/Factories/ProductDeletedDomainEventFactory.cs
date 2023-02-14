using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductDeleted.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductDeleted.Factories;
public class ProductDeletedDomainEventFactory
    : DomainEventFactoryBase,
    IProductDeletedDomainEventFactory
{
    // Constructors
    public ProductDeletedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public ProductDeletedDomainEvent? Create((Product Product, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<ProductDeletedDomainEvent>();

        return new ProductDeletedDomainEvent(
            correlationId: parameter.CorrelationId,
            id: id,
            tenantId: parameter.Product.TenantId,
            timestamp: timestamp,
            executionUser: parameter.ExecutionUser,
            sourcePlatform: parameter.SourcePlatform,
            domainEventType: domainEventType,
            parameter.Product
        );
    }
}
