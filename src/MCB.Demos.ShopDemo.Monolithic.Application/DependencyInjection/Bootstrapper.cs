using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Customers.GetCustomerByEmail;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Customers.GetCustomerByEmail.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.RemoveCustomer;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.RemoveCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomer;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomerBatch;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.RemoveProduct;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.RemoveProduct.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Factories
        dependencyInjectionContainer.RegisterSingleton<IExternalEventFactory, ExternalEventFactory>();

        ConfigureDependencyInjectionForCustomer(dependencyInjectionContainer);
        ConfigureDependencyInjectionForProduct(dependencyInjectionContainer);
    }

    private static void ConfigureDependencyInjectionForCustomer(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Use Cases
        dependencyInjectionContainer.RegisterScoped<IImportCustomerUseCase, ImportCustomerUseCase>();
        dependencyInjectionContainer.RegisterScoped<IImportCustomerBatchUseCase, ImportCustomerBatchUseCase>();
        dependencyInjectionContainer.RegisterScoped<IValidateImportCustomerBatchUseCase, ValidateImportCustomerBatchUseCase>();
        dependencyInjectionContainer.RegisterScoped<IRemoveCustomerUseCase, RemoveCustomerUseCase>();

        // Queries
        dependencyInjectionContainer.RegisterScoped<IGetCustomerByEmailQuery, GetCustomerByEmailQuery>();
    }
    private static void ConfigureDependencyInjectionForProduct(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Use Cases
        dependencyInjectionContainer.RegisterScoped<IImportProductUseCase, ImportProductUseCase>();
        dependencyInjectionContainer.RegisterScoped<IImportProductBatchUseCase, ImportProductBatchUseCase>();
        dependencyInjectionContainer.RegisterScoped<IValidateImportProductBatchUseCase, ValidateImportProductBatchUseCase>();
        dependencyInjectionContainer.RegisterScoped<IRemoveProductUseCase, RemoveProductUseCase>();

        // Queries
        dependencyInjectionContainer.RegisterScoped<IGetProductByCodeQuery, GetProductByCodeQuery>();
    }
}