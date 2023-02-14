using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductDeleted.Factories.Interfaces;
public interface IProductDeletedDomainEventFactory
    : IFactoryWithParameter<ProductDeletedDomainEvent, (Product Product, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}
