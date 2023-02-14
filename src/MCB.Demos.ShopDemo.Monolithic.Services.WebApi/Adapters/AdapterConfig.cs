using Mapster;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Payloads;

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

        ConfigureMapFromDomainEntityToDto<Customer, CustomerDto>();
        ConfigureMapFromDomainEntityToDto<Product, ProductDto>();
    }

    // Private Methods
    private static void ConfigureMapFromDomainEntityToDto<TDomainEntity, TDto>()
        where TDomainEntity : IDomainEntity
        where TDto : DtoBase
    {
        TypeAdapterConfig<TDomainEntity, TDto>.NewConfig()
            .Map(dest => dest.CreatedBy, src => src.AuditableInfo.CreatedBy)
            .Map(dest => dest.CreatedAt, src => src.AuditableInfo.CreatedAt)
            .Map(dest => dest.LastUpdatedBy, src => src.AuditableInfo.LastUpdatedBy)
            .Map(dest => dest.LastUpdatedAt, src => src.AuditableInfo.LastUpdatedAt)
            .Map(dest => dest.LastSourcePlatform, src => src.AuditableInfo.LastSourcePlatform)
            .Map(dest => dest.LastCorrelationId, src => src.AuditableInfo.LastCorrelationId)
            ;
    }
    private static void ConfigureForWebApi()
    {
        // Customer
        TypeAdapterConfig<ImportCustomerPayload, ImportCustomerUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayloadItem, ImportCustomerBatchUseCaseInputItem>.NewConfig();
        TypeAdapterConfig<ValidateImportCustomerBatchPayload, ValidateImportCustomerBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ValidateImportCustomerBatchPayloadItem, ValidateImportCustomerBatchUseCaseInputItem>.NewConfig();

        // Product
        TypeAdapterConfig<ImportProductPayload, ImportProductUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportProductBatchPayload, ImportProductBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportProductBatchPayloadItem, ImportProductBatchUseCaseInputItem>.NewConfig();
        TypeAdapterConfig<ValidateImportProductBatchPayload, ValidateImportProductBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ValidateImportProductBatchPayloadItem, ValidateImportProductBatchUseCaseInputItem>.NewConfig();
    }
}