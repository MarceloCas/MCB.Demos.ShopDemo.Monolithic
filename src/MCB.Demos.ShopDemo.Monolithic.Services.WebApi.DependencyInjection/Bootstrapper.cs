using Mapster;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Enums;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.DependencyInjection;

public static class Bootstrapper
{
    // Public Methods
    public static void ConfigureDependencyInjection(
        string applicationName,
        string? applicationVersion,
        IDependencyInjectionContainer dependencyInjectionContainer,
        Action<TypeAdapterConfig> adapterMapAction,
        AppSettings appSettings
    )
    {
        // Inject Dependencies
        Core.Infra.CrossCutting.Observability.OpenTelemetry.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(
            dependencyInjectionContainer, 
            applicationName,
            applicationVersion
        );
        Core.Infra.CrossCutting.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer);
        Core.Infra.CrossCutting.DesignPatterns.DependencyInjection.Bootstrapper.ConfigureServices(
            dependencyInjectionContainer,
            adapterConfiguration =>
            {
                adapterConfiguration.DependencyInjectionLifecycle = DependencyInjectionLifecycle.Singleton;
                adapterConfiguration.TypeAdapterConfigurationFunction = new Func<TypeAdapterConfig>(() =>
                {
                    var typeAdapterConfig = new TypeAdapterConfig();

                    adapterMapAction(typeAdapterConfig);

                    return typeAdapterConfig;
                });
            }
        );
        Core.Domain.DependencyInjection.Bootstrapper.ConfigureServices(dependencyInjectionContainer);

        // Inject Layers
        Application.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer);
        Domain.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer);
        Domain.Entities.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer);
        Infra.Data.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer, appSettings);
        Infra.CrossCutting.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer, appSettings);
        Infra.CrossCutting.FeatureFlag.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer);
    }
}