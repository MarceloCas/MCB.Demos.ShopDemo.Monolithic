using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModelRepositories;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModelRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;
using MongoDB.Driver;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(IDependencyInjectionContainer dependencyInjectionContainer, AppSettings appSettings)
    {
        // Unit of Work
        dependencyInjectionContainer.RegisterScoped<IUnitOfWork, DefaultUnitOfWork>();

        // Data Contexts - MongoDb
        ConfigureDependencyInjectionForMongoDb(dependencyInjectionContainer, appSettings);

        // Data Contexts - Redis
        ConfigureDependencyInjectionForRedis(dependencyInjectionContainer, appSettings);

        // Repositories
        dependencyInjectionContainer.RegisterScoped<ICustomerRepository, CustomerRepository>();
    }

    // Private Methods
    private static void ConfigureDependencyInjectionForMongoDb(IDependencyInjectionContainer dependencyInjectionContainer, AppSettings appSettings)
    {
        // Data Context
        dependencyInjectionContainer.RegisterSingleton(dependencyInjectionContainer =>
                    new MongoDbOptions(
                        connectionString: appSettings.MongoDb.ConnectionString,
                        databaseName: appSettings.MongoDb.DatabaseName,
                        mongoDatabaseSettings: null,
                        mongoDbClientSessionOptions: null
                    )
                );
        dependencyInjectionContainer.RegisterSingleton(dependencyInjectionContainer =>
            new MongoClient(
                connectionString: dependencyInjectionContainer.Resolve<MongoDbOptions>()!.ConnectionString
            )
        );
        dependencyInjectionContainer.RegisterScoped<IDefaultMongoDbDataContext>(dependencyInjectionContainer =>
        {
            var dataContext = new DefaultMongoDbDataContext(
                client: dependencyInjectionContainer.Resolve<MongoClient>()!,
                options: dependencyInjectionContainer.Resolve<MongoDbOptions>()!
            );

            dataContext.Init();

            return dataContext;
        });

        // DataModelRepositories
        dependencyInjectionContainer.RegisterScoped<ICustomerMongoDbDataModelRepository, CustomerMongoDbDataModelRepository>();
    }
    private static void ConfigureDependencyInjectionForRedis(IDependencyInjectionContainer dependencyInjectionContainer, AppSettings appSettings)
    {
        // DataContext
        dependencyInjectionContainer.RegisterSingleton<IDefaultRedisDataContext>(dependencyInjectionContainer =>
            new DefaultRedisDataContext(
                new RedisOptions(
                    connectionString: appSettings.Redis.ConnectionString
                )
            )
        );
    }

}