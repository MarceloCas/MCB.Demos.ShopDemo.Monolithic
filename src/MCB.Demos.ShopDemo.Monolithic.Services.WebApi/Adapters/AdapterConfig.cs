using Mapster;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;

public class AdapterConfig
{
    // Public Methods
    public static void Configure(TypeAdapterConfig typeAdapterConfig, IDependencyInjectionContainer dependencyInjectionContainer)
    {
        ConfigureForWebApi();

        Application.Adapters.AdapterConfig.Configure(typeAdapterConfig);
        Domain.Adapters.AdapterConfig.Configure(typeAdapterConfig);
        Infra.Data.Adapters.AdapterConfig.Configure(typeAdapterConfig, dependencyInjectionContainer);
    }

    // Private Methods
    private static void ConfigureForWebApi()
    {
        TypeAdapterConfig<ImportCustomerPayload, RegisterNewCustomerUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayload, RegisterNewCustomerBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayloadItem, RegisterNewCustomerBatchUseCaseInputItem>.NewConfig();
    }
}