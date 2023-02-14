using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerImported.Factories.Interfaces;

public interface ICustomerImportedDomainEventFactory
    : IFactoryWithParameter<CustomerImportedDomainEvent, (Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}