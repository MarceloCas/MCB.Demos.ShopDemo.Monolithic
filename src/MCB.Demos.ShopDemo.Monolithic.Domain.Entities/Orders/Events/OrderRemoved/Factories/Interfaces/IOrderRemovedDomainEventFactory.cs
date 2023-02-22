using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderRemoved.Factories.Interfaces;
public interface IOrderRemovedDomainEventFactory
    : IFactoryWithParameter<OrderRemovedDomainEvent, (Order Order, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}
