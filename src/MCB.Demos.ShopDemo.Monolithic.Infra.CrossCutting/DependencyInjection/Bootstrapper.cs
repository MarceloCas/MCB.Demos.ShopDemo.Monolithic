using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.RabbitMq.Connection.Interfaces;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;

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
                )
            )
        );
    }
}
