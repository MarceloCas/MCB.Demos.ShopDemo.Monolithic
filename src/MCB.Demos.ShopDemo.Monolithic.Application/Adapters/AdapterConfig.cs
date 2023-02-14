using Mapster;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerImported;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductDeleted;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Customers.CustomerDeleted;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Customers.CustomerImported;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductDeleted;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductImported;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Adapters;

public static class AdapterConfig
{
    // Public Methods
    public static void Configure()
    {
        ConfigureForCustomer();
        ConfigureForProduct();
    }

    // Private Methods
    private static void ConfigureForCustomer()
    {
        TypeAdapterConfig<ImportCustomerUseCaseInput, ImportCustomerServiceInput>.NewConfig();

        TypeAdapterConfig<(ImportCustomerBatchUseCaseInput, ImportCustomerBatchUseCaseInputItem), ImportCustomerServiceInput>.NewConfig()
            .MapWith(src =>
                new ImportCustomerServiceInput(
                    src.Item1.CorrelationId,
                    src.Item1.TenantId,
                    src.Item2.FirstName ?? string.Empty,
                    src.Item2.LastName ?? string.Empty,
                    src.Item2.BirthDate ?? default,
                    src.Item2.Email ?? string.Empty,
                    src.Item1.ExecutionUser ?? string.Empty,
                    src.Item1.SourcePlatform ?? string.Empty
                )
            );
        TypeAdapterConfig<(ValidateImportCustomerBatchUseCaseInput, ValidateImportCustomerBatchUseCaseInputItem), ValidateImportCustomerServiceInput>.NewConfig()
            .MapWith(src =>
                new ValidateImportCustomerServiceInput(
                    src.Item1.CorrelationId,
                    src.Item1.TenantId,
                    src.Item2.FirstName ?? string.Empty,
                    src.Item2.LastName ?? string.Empty,
                    src.Item2.BirthDate ?? default,
                    src.Item2.Email ?? string.Empty,
                    src.Item1.ExecutionUser ?? string.Empty,
                    src.Item1.SourcePlatform ?? string.Empty
                )
            );

        // ExternalEvents
        TypeAdapterConfig<CustomerImportedDomainEvent, CustomerImportedEvent>.NewConfig().Map(dest => dest.Customer, src => ((Domain.Entities.Customers.Customer)src.AggregationRoot).Adapt<CustomerDto>());
        TypeAdapterConfig<CustomerDeletedDomainEvent, CustomerDeletedEvent>.NewConfig().Map(dest => dest.Customer, src => ((Domain.Entities.Customers.Customer)src.AggregationRoot).Adapt<CustomerDto>());

        MapDomainEntityToDto<Domain.Entities.Customers.Customer, CustomerDto>();
    }
    private static void ConfigureForProduct()
    {
        TypeAdapterConfig<ImportProductUseCaseInput, ImportProductServiceInput>.NewConfig();

        TypeAdapterConfig<(ImportProductBatchUseCaseInput, ImportProductBatchUseCaseInputItem), ImportProductServiceInput>.NewConfig()
            .MapWith(src =>
                new ImportProductServiceInput(
                    src.Item1.CorrelationId,
                    src.Item1.TenantId,
                    src.Item2.Code ?? string.Empty,
                    src.Item2.Description ?? string.Empty,
                    src.Item1.ExecutionUser ?? string.Empty,
                    src.Item1.SourcePlatform ?? string.Empty
                )
            );
        TypeAdapterConfig<(ValidateImportProductBatchUseCaseInput, ValidateImportProductBatchUseCaseInputItem), ValidateImportProductServiceInput>.NewConfig()
            .MapWith(src =>
                new ValidateImportProductServiceInput(
                    src.Item1.CorrelationId,
                    src.Item1.TenantId,
                    src.Item2.Code ?? string.Empty,
                    src.Item2.Description ?? string.Empty,
                    src.Item1.ExecutionUser ?? string.Empty,
                    src.Item1.SourcePlatform ?? string.Empty
                )
            );

        // ExternalEvents
        TypeAdapterConfig<ProductImportedDomainEvent, ProductImportedEvent>.NewConfig().Map(dest => dest.Product, src => ((Domain.Entities.Products.Product)src.AggregationRoot).Adapt<ProductDto>());
        TypeAdapterConfig<ProductDeletedDomainEvent, ProductDeletedEvent>.NewConfig().Map(dest => dest.Product, src => ((Domain.Entities.Products.Product)src.AggregationRoot).Adapt<ProductDto>());

        MapDomainEntityToDto<Domain.Entities.Products.Product, ProductDto>();
    }

    private static void MapDomainEntityToDto<TAggregationRoot, TDtoBase>()
        where TAggregationRoot : IAggregationRoot
        where TDtoBase : DtoBase
    {
        TypeAdapterConfig<TAggregationRoot, TDtoBase>.NewConfig()
            .Map(dest => dest.CreatedAt, src => src.AuditableInfo.CreatedAt)
            .Map(dest => dest.CreatedBy, src => src.AuditableInfo.CreatedBy)
            .Map(dest => dest.LastUpdatedAt, src => src.AuditableInfo.LastUpdatedAt)
            .Map(dest => dest.LastUpdatedBy, src => src.AuditableInfo.LastUpdatedBy)
            .Map(dest => dest.LastSourcePlatform, src => src.AuditableInfo.LastSourcePlatform);
    }
}
