using Mapster;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;
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

        ConfigureMapFromDomainEntityToDto<Customer, CustomerDto>();
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
        TypeAdapterConfig<ImportCustomerPayload, ImportCustomerUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ImportCustomerBatchPayloadItem, ImportCustomerBatchUseCaseInputItem>.NewConfig();
        TypeAdapterConfig<ValidateImportCustomerBatchPayload, ValidateImportCustomerBatchUseCaseInput>.NewConfig();
        TypeAdapterConfig<ValidateImportCustomerBatchPayloadItem, ValidateImportCustomerBatchUseCaseInputItem>.NewConfig();
    }
}