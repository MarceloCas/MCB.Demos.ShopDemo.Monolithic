using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Factories
        dependencyInjectionContainer.RegisterSingleton<ICustomerFactory, CustomerFactory>();
        dependencyInjectionContainer.RegisterSingleton<ICustomerHasBeenRegisteredDomainEventFactory, CustomerHasBeenRegisteredDomainEventFactory>();
    }
}