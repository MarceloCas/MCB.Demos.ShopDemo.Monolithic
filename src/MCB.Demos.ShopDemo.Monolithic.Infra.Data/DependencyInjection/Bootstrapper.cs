using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;
using StackExchange.Redis;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DependencyInjection;

public static class Bootstrapper
{
    // Properties
    public static void ConfigureDependencyInjection(
        IDependencyInjectionContainer dependencyInjectionContainer,
        AppSettings appSettings
    )
    {
        // Unit of Work
        dependencyInjectionContainer.RegisterScoped<IUnitOfWork, DefaultUnitOfWork>();

        // Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerRepository, CustomerRepository>();
        dependencyInjectionContainer.RegisterScoped<IProductRepository, ProductRepository>();

        ConfigureDependencyInjectionForEntityFramework(dependencyInjectionContainer);
        ConfigureDependencyInjectionForRedis(dependencyInjectionContainer, appSettings);

        // Resilience Policies
        dependencyInjectionContainer.RegisterSingleton<IPostgreSqlResiliencePolicy, PostgreSqlResiliencePolicy>();
        dependencyInjectionContainer.RegisterSingleton<IRedisResiliencePolicy, RedisResiliencePolicy>();
    }

    // Private Methods
    private static void ConfigureDependencyInjectionForEntityFramework(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Default Entity Framework Data Context is configured in WebApi project to use IServiceCollection EF Core extension methods

        // Data Model Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerDataModelEntityFrameworkRepository, CustomerDataModelEntityFrameworkRepository>();
        dependencyInjectionContainer.RegisterScoped<IProductDataModelEntityFrameworkRepository, ProductDataModelEntityFrameworkRepository>();
    }
    private static void ConfigureDependencyInjectionForRedis(IDependencyInjectionContainer dependencyInjectionContainer, AppSettings appSettings)
    {
        // DataContext
        dependencyInjectionContainer.RegisterSingleton<IRedisDataContext>(dependencyInjectionContainer => {
            var dataContext = new DefaultRedisDataContext(
                new RedisOptions(
                    connectionString: appSettings.Redis.ConnectionString
                ),
                dependencyInjectionContainer.Resolve<IRedisResiliencePolicy>()!
            );

            dataContext.TryOpenConnectionAsync(cancellationToken: default).GetAwaiter().GetResult();

            return dataContext;
        });

        // Connection
        dependencyInjectionContainer.RegisterSingleton<IConnectionMultiplexer>(dependencyInjectionContainer =>
        {
            return dependencyInjectionContainer.Resolve<IRedisDataContext>()!.ConnectionMultiplexer;
        });

        // DataModels Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerDataModelRedisRepository, CustomerDataModelRedisRepository>();
        dependencyInjectionContainer.RegisterScoped<IProductDataModelRedisRepository, ProductDataModelRedisRepository>();
    }

}