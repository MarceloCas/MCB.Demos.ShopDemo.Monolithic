using Mapster;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Adapters;

public class AdapterConfig
{
    // Public Methods
    public static void Configure(TypeAdapterConfig typeAdapterConfig, IDependencyInjectionContainer dependencyInjectionContainer)
    {
        MapDomainEntityToDataModel();
        MapDataModelToDomainEntity(dependencyInjectionContainer);
    }

    // Private Methods
    private static void MapDomainEntityToDataModel()
    {
        ConfigureMapFromDomainEntityBaseToDataModelBase<Customer, CustomerDataModel>(config =>
        {
            config.Map(dest => dest.Email, src => src.Email);
        });
    }
    private static void MapDataModelToDomainEntity(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        TypeAdapterConfig<CustomerDataModel, SetExistingCustomerInfoInput>.NewConfig();
        TypeAdapterConfig<CustomerDataModel, Customer>.NewConfig()
            .MapWith(converterFactory: src =>
                dependencyInjectionContainer
                    .Resolve<ICustomerFactory>()!
                    .Create()!
                    .SetExistingCustomerInfo(src.Adapt<SetExistingCustomerInfoInput>())
            );
    }
    private static void ConfigureMapFromDomainEntityBaseToDataModelBase<TDomainEntityBase, TDataModelBase>(
        Action<TypeAdapterSetter<TDomainEntityBase, TDataModelBase>>? additionalMapConfigHandler = null
    )   where TDomainEntityBase : DomainEntityBase
        where TDataModelBase : DataModelBase
    {
        var config = TypeAdapterConfig<TDomainEntityBase, TDataModelBase>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.TenantId, src => src.TenantId)
            .Map(dest => dest.CreatedBy, src => src.AuditableInfo.CreatedBy)
            .Map(dest => dest.CreatedAt, src => src.AuditableInfo.CreatedAt)
            .Map(dest => dest.LastUpdatedBy, src => src.AuditableInfo.LastUpdatedBy)
            .Map(dest => dest.LastUpdatedAt, src => src.AuditableInfo.LastUpdatedAt)
            .Map(dest => dest.LastSourcePlatform, src => src.AuditableInfo.LastSourcePlatform)
            .Map(dest => dest.RegistryVersion, src => src.RegistryVersion);

        additionalMapConfigHandler?.Invoke(config);
    }
}