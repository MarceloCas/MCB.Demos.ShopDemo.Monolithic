using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported.Factories.Interfaces;

public interface IProductImportedDomainEventFactory
    : IFactoryWithParameter<ProductImportedDomainEvent, (Product Product, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}
