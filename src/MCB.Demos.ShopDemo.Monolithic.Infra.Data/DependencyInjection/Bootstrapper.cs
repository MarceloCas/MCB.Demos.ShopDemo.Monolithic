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

        // Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerRepository, CustomerRepository>();

        ConfigureDependencyInjectionForEntityFramework(dependencyInjectionContainer, appSettings);
        ConfigureDependencyInjectionForRedis(dependencyInjectionContainer, appSettings);

        // Resilience Policies
        dependencyInjectionContainer.RegisterSingleton<IPostgreSqlResiliencePolicy, PostgreSqlResiliencePolicy>();
        dependencyInjectionContainer.RegisterSingleton<IRedisResiliencePolicy, RedisResiliencePolicy>();
    }

    // Private Methods
    private static void ConfigureDependencyInjectionForEntityFramework(IDependencyInjectionContainer dependencyInjectionContainer, AppSettings appSettings)
    {
        // Default Entity Framework Data Context is configured in WebApi project to use IServiceCollection EF Core extension methods

        // Data Model Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerDataModelEntityFrameworkRepository, CustomerDataModelEntityFrameworkRepository>();
    }
    private static void ConfigureDependencyInjectionForRedis(IDependencyInjectionContainer dependencyInjectionContainer, AppSettings appSettings)
    {
        // DataContext
        dependencyInjectionContainer.RegisterSingleton<IRedisDataContext>(dependencyInjectionContainer =>
            new DefaultRedisDataContext(
                new RedisOptions(
                    connectionString: appSettings.Redis.ConnectionString
                ),
                dependencyInjectionContainer.Resolve<IRedisResiliencePolicy>()!
            )
        );

        // DataModels Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerDataModelRedisRepository, CustomerDataModelRedisRepository>();
    }

}