using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductRemoved.Factories.Interfaces;
public interface IProductRemovedDomainEventFactory
    : IFactoryWithParameter<ProductRemovedDomainEvent, (Product Product, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}
