﻿using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Core.Infra.CrossCutting.RabbitMq.Connection.Interfaces;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models.Enums;
using MCB.Core.Infra.CrossCutting.Serialization;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Customers.CustomerRemoved;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Customers.CustomerImported;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductRemoved;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductImported;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.DependencyInjection;
public static class Bootstrapper
{
    public static void ConfigureDependencyInjection(
        IDependencyInjectionContainer dependencyInjectionContainer,
        AppSettings appSettings
    )
    {
        // RabbitMq
        dependencyInjectionContainer.RegisterSingleton<IRabbitMqConnection, RabbitMqConnection>(d =>
            new RabbitMqConnection(
                new RabbitMqConnectionConfig(
                    ClientProvidedName: appSettings.RabbitMq.Connection.ClientProvidedName,
                    HostName: appSettings.RabbitMq.Connection.HostName,
                    Port: appSettings.RabbitMq.Connection.Port,
                    Username: appSettings.RabbitMq.Connection.Username,
                    Password: appSettings.RabbitMq.Connection.Password,
                    VirtualHost: appSettings.RabbitMq.Connection.VirtualHost,
                    DispatchConsumersAsync: appSettings.RabbitMq.Connection.DispatchConsumersAsync,
                    AutoConnect: appSettings.RabbitMq.Connection.AutoConnect,
                    AutomaticRecoveryEnabled: appSettings.RabbitMq.Connection.AutomaticRecoveryEnabled,
                    NetworkRecoveryInterval: TimeSpan.FromSeconds(appSettings.RabbitMq.Connection.NetworkRecoveryIntervalSeconds),
                    TopologyRecoveryEnabled: appSettings.RabbitMq.Connection.TopologyRecoveryEnabled,
                    RequestedHeartbeat: TimeSpan.FromSeconds(appSettings.RabbitMq.Connection.RequestedHeartbeatSeconds)
                ),
                dependencyInjectionContainer.Resolve<ITraceManager>()!,
                dependencyInjectionContainer.Resolve<IRabbitMqResiliencePolicy>()!
            )
        );
        dependencyInjectionContainer.RegisterSingleton<IEventsExchangeRabbitMqPublisher>(d =>
        {
            return new EventsExchangeRabbitMqPublisher(
                connection: d.Resolve<IRabbitMqConnection>()!,
                exchangeConfig: new RabbitMqExchangeConfig(
                    ExchangeName: appSettings.RabbitMq.EventsExchange.Name,
                    ExchangeType: RabbitMqExchangeType.Header,
                    Durable: appSettings.RabbitMq.EventsExchange.Durable,
                    AutoDelete: appSettings.RabbitMq.EventsExchange.AutoRemove,
                    Arguments: null
                ),
                traceManager: d.Resolve<ITraceManager>()!,
                rabbitMqResiliencePolicy: d.Resolve<IRabbitMqResiliencePolicy>()!,
                protobufSerializer: d.Resolve<IProtobufSerializer>()!
            );
        });

        dependencyInjectionContainer.RegisterSingleton<IRabbitMqResiliencePolicy, RabbitMqResiliencePolicy>();

        ConfigureProtobufSerializationForMessages();
    }

    private static void ConfigureProtobufSerializationForMessages()
    {
        // Protobuf Serialization
        ProtobufSerializer.ConfigureTypeCollection(new[] {
            // Customers
            typeof(CustomerDto),
            typeof(CustomerImportedEvent),
            typeof(CustomerRemovedEvent),
            // Products
            typeof(ProductDto),
            typeof(ProductImportedEvent),
            typeof(ProductRemovedEvent),
        });
    }
}
