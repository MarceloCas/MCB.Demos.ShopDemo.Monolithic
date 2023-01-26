using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.DependencyInjection;
public static class Bootstrapper
{
    public static void ConfigureDependencyInjection(
        IDependencyInjectionContainer dependencyInjectionContainer
    )
    {
        dependencyInjectionContainer.RegisterSingleton<IMcbFeatureFlagManager, ConsulKvMcbFlagManager>();
    }
}
