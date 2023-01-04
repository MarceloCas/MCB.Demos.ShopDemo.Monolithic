using Mapster;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;

public static class AdapterConfig
{
    // Public Methods
    public static void Configure(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        ConfigureForWebApi();

        Application.Adapters.AdapterConfig.Configure();
        Domain.Adapters.AdapterConfig.Configure();
        Infra.Data.Adapters.AdapterConfig.Configure(dependencyInjectionContainer);

        TypeAdapterConfig<Customer, CustomerDto>.NewConfig()
            .Map(dest => dest.CreatedBy, src => src.AuditableInfo.CreatedBy)
            .Map(dest => dest.CreatedAt, src => src.AuditableInfo.CreatedAt)
            .Map(dest => dest.LastUpdatedBy, src => src.AuditableInfo.LastUpdatedBy)
            .Map(dest => dest.LastUpdatedAt, src => src.AuditableInfo.LastUpdatedAt)
            .Map(dest => dest.LastSourcePlatform, src => src.AuditableInfo.LastSourcePlatform)
            ;

    }

    // Private Methods
    private static void ConfigureForWebApi()
    {
        TypeAdapterConfig<ImportCustomerPayload, ImportCustomerUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayloadItem, ImportCustomerBatchUseCaseInputItem>.NewConfig();
    }
}