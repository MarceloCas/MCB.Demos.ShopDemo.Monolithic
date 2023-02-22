using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductRemoved.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductRemoved.Factories;
public class ProductRemovedDomainEventFactory
    : DomainEventFactoryBase,
    IProductRemovedDomainEventFactory
{
    // Constructors
    public ProductRemovedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public ProductRemovedDomainEvent? Create((Product Product, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<ProductRemovedDomainEvent>();

        return new ProductRemovedDomainEvent(
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
