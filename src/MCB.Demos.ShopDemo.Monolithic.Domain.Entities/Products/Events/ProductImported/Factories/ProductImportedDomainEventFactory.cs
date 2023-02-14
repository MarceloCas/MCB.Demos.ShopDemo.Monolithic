using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported.Factories;
public class ProductImportedDomainEventFactory
    : DomainEventFactoryBase,
    IProductImportedDomainEventFactory
{
    // Constructors
    public ProductImportedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public ProductImportedDomainEvent Create((Product Product, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<ProductImportedDomainEvent>();

        return new ProductImportedDomainEvent(
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