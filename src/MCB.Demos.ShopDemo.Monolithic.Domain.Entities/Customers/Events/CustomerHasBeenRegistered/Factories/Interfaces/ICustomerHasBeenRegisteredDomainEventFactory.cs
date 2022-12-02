using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered.Factories.Interfaces;

public interface ICustomerHasBeenRegisteredDomainEventFactory
    : IFactoryWithParameter<CustomerHasBeenRegisteredDomainEvent, (Customer customer, string executionUser, string sourcePlatform)>
{
}