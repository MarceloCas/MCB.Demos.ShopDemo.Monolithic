using MCB.Core.Infra.CrossCutting.DesignPatterns.Resilience;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;
using Microsoft.Extensions.Logging;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies;

public class RabbitMqResiliencePolicy
    : ResiliencePolicyBase,
    IRabbitMqResiliencePolicy
{
    // Constructors
    public RabbitMqResiliencePolicy(
        ILogger<RabbitMqResiliencePolicy> logger
    ) : base(logger)
    {
    }
}