using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRemoved.Factories.Interfaces;
public interface ICustomerRemovedDomainEventFactory
    : IFactoryWithParameter<CustomerRemovedDomainEvent, (Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}
