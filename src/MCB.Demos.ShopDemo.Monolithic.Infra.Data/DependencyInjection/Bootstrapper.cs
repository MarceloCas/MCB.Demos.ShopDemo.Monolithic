﻿using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(
        IDependencyInjectionContainer dependencyInjectionContainer, 
        AppSettings appSettings
    )
    {
        // Unit of Work
        dependencyInjectionContainer.RegisterScoped<IUnitOfWork, DefaultUnitOfWork>();

        // Default Entity Framework Data Context is configured in WebApi project to use IServiceCollection EF Core extension methods

        // Data Contexts - Redis
        ConfigureDependencyInjectionForRedis(dependencyInjectionContainer, appSettings);

        // Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerRepository, CustomerRepository>();

        // DataModels Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerDataModelRepository, CustomerDataModelRedisRepository>();
        dependencyInjectionContainer.RegisterScoped<CustomerDataModelEntityFrameworkRepository>();
    }

    // Private Methods
    private static void ConfigureDependencyInjectionForRedis(IDependencyInjectionContainer dependencyInjectionContainer, AppSettings appSettings)
    {
        // DataContext
        dependencyInjectionContainer.RegisterSingleton<IRedisDataContext>(dependencyInjectionContainer =>
            new DefaultRedisDataContext(
                new RedisOptions(
                    connectionString: appSettings.Redis.ConnectionString
                )
            )
        );
    }

}