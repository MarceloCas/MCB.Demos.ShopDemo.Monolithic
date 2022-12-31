using Mapster;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;

public class AdapterConfig
{
    // Public Methods
    public static void Configure(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        ConfigureForWebApi();

        Application.Adapters.AdapterConfig.Configure();
        Domain.Adapters.AdapterConfig.Configure();
        Infra.Data.Adapters.AdapterConfig.Configure(dependencyInjectionContainer);
    }

    // Private Methods
    private static void ConfigureForWebApi()
    {
        TypeAdapterConfig<ImportCustomerPayload, ImportCustomerUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayloadItem, ImportCustomerBatchUseCaseInputItem>.NewConfig();
    }
}