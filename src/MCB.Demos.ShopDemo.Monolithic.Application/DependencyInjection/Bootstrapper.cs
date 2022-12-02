using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Factories
        dependencyInjectionContainer.RegisterSingleton<IExternalEventFactory, ExternalEventFactory>();

        // Use Cases
        dependencyInjectionContainer.RegisterScoped<IRegisterNewCustomerUseCase, RegisterNewCustomerUseCase>();
    }
}