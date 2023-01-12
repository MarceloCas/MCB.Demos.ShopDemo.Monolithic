using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;

public interface IRabbitMqResiliencePolicy
    : IResiliencePolicy
{
}
