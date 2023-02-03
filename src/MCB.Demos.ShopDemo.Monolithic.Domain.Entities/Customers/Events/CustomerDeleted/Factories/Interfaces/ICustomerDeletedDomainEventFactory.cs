using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories.Interfaces;
public interface ICustomerDeletedDomainEventFactory
    : IFactoryWithParameter<CustomerDeletedDomainEvent, (Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}
