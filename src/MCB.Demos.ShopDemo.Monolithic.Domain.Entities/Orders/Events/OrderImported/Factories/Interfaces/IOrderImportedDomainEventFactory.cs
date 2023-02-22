using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderImported.Factories.Interfaces;
public interface IOrderImportedDomainEventFactory
    : IFactoryWithParameter<OrderImportedDomainEvent, (Order Order, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}
