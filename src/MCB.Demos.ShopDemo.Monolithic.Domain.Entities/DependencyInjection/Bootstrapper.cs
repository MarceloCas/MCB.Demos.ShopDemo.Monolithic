using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerImported.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerImported.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderImported.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderImported.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderRemoved.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderRemoved.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductDeleted.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductDeleted.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        ConfigureDependencyInjectionForCustomer(dependencyInjectionContainer);
        ConfigureDependencyInjectionForProduct(dependencyInjectionContainer);
        ConfigureDependencyInjectionForOrder(dependencyInjectionContainer);
    }

    // Private Methods
    private static void ConfigureDependencyInjectionForCustomer(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Factories
        dependencyInjectionContainer.RegisterSingleton<ICustomerFactory, CustomerFactory>();

        // Domain Events
        dependencyInjectionContainer.RegisterSingleton<ICustomerImportedDomainEventFactory, CustomerImportedDomainEventFactory>();
        dependencyInjectionContainer.RegisterSingleton<ICustomerDeletedDomainEventFactory, CustomerDeletedDomainEventFactory>();
    }
    private static void ConfigureDependencyInjectionForProduct(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Factories
        dependencyInjectionContainer.RegisterSingleton<IProductFactory, ProductFactory>();

        // Domain Events
        dependencyInjectionContainer.RegisterSingleton<IProductImportedDomainEventFactory, ProductImportedDomainEventFactory>();
        dependencyInjectionContainer.RegisterSingleton<IProductDeletedDomainEventFactory, ProductDeletedDomainEventFactory>();
    }
    private static void ConfigureDependencyInjectionForOrder(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Factories
        dependencyInjectionContainer.RegisterSingleton<IOrderFactory, OrderFactory>();
        dependencyInjectionContainer.RegisterSingleton<IOrderItemFactory, OrderItemFactory>();

        // Domain Events
        dependencyInjectionContainer.RegisterSingleton<IOrderImportedDomainEventFactory, OrderImportedDomainEventFactory>();
        dependencyInjectionContainer.RegisterSingleton<IOrderRemovedDomainEventFactory, OrderRemovedDomainEventFactory>();
    }
}