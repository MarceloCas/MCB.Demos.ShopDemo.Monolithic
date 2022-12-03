﻿using Mapster;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.CustomerHasBeenRegistered;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Adapters;

public class AdapterConfig
{
    // Public Methods
    public static void Configure(TypeAdapterConfig typeAdapterConfig)
    {
        typeAdapterConfig.ForType<RegisterNewCustomerUseCaseInput, RegisterNewCustomerServiceInput>();

        // ExternalEvents
        typeAdapterConfig.ForType<CustomerHasBeenRegisteredDomainEvent, CustomerHasBeenRegisteredEvent>()
            .Map(dest => dest.Customer, src => ((Domain.Entities.Customers.Customer)src.AggregationRoot).Adapt<CustomerDto>());

        MapDomainEntityToDto<Domain.Entities.Customers.Customer, CustomerDto>();
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