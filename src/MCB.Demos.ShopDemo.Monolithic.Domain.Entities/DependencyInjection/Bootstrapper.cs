using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRegistered.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRegistered.Factories.Interfaces;
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

        dependencyInjectionContainer.RegisterSingleton<ICustomerRegisteredDomainEventFactory, CustomerRegisteredDomainEventFactory>();
        dependencyInjectionContainer.RegisterSingleton<ICustomerDeletedDomainEventFactory, CustomerDeletedDomainEventFactory>();
    }
}