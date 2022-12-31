using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Factories
        dependencyInjectionContainer.RegisterSingleton<IExternalEventFactory, ExternalEventFactory>();

        // Use Cases
        dependencyInjectionContainer.RegisterScoped<IImportCustomerUseCase, ImportCustomerUseCase>();
        dependencyInjectionContainer.RegisterScoped<IImportCustomerBatchUseCase, ImportCustomerBatchUseCase>();
    }
}