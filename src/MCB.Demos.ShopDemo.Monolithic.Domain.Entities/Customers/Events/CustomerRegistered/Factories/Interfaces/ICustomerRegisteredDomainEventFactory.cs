using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRegistered.Factories.Interfaces;

public interface ICustomerRegisteredDomainEventFactory
    : IFactoryWithParameter<CustomerRegisteredDomainEvent, (Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId)>
{
}