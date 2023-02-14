using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Services
        dependencyInjectionContainer.RegisterScoped<ICustomerService, CustomerService>();
        dependencyInjectionContainer.RegisterScoped<IProductService, ProductService>();
    }
}