using Mapster;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Adapters;

public static class AdapterConfig
{
    // Public Methods
    public static void Configure(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        MapDomainEntityToDataModel();
        MapDataModelToDomainEntity(dependencyInjectionContainer);
    }

    // Private Methods
    private static void MapDomainEntityToDataModel()
    {
        ConfigureMapFromDomainEntityBaseToDataModelBase<Customer, CustomerDataModel>();
    }
    private static void MapDataModelToDomainEntity(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        ConfigureMapFromDataModelToInputBase<CustomerDataModel, SetExistingCustomerInfoInput>();
        TypeAdapterConfig<CustomerDataModel, Customer>.NewConfig()
            .MapWith(converterFactory: src =>
                dependencyInjectionContainer
                    .Resolve<ICustomerFactory>()!
                    .Create()!
                    .SetExistingCustomerInfo(src.Adapt<SetExistingCustomerInfoInput>())
            );
    }
    private static void ConfigureMapFromDomainEntityBaseToDataModelBase<TDomainEntityBase, TDataModelBase>()   
        where TDomainEntityBase : DomainEntityBase
        where TDataModelBase : DataModelBase
    {
        TypeAdapterConfig<TDomainEntityBase, TDataModelBase>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.TenantId, src => src.TenantId)
            .Map(dest => dest.CreatedBy, src => src.AuditableInfo.CreatedBy)
            .Map(dest => dest.CreatedAt, src => src.AuditableInfo.CreatedAt)
            .Map(dest => dest.LastUpdatedBy, src => src.AuditableInfo.LastUpdatedBy)
            .Map(dest => dest.LastUpdatedAt, src => src.AuditableInfo.LastUpdatedAt)
            .Map(dest => dest.LastSourcePlatform, src => src.AuditableInfo.LastSourcePlatform)
            .Map(dest => dest.LastCorrelationId, src => src.AuditableInfo.LastCorrelationId)
            .Map(dest => dest.RegistryVersion, src => src.RegistryVersion);
    }
    private static void ConfigureMapFromDataModelToInputBase<TDataModel, TInput>()
        where TDataModel : DataModelBase
        where TInput : InputBase
    {
        TypeAdapterConfig<TDataModel, TInput>.NewConfig()
            .Map(dest => dest.CorrelationId, src => src.LastCorrelationId);
    }
}