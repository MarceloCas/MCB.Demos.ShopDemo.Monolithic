using MCB.Core.Infra.CrossCutting.DesignPatterns.Resilience;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using Microsoft.Extensions.Logging;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies;

public class RedisResiliencePolicy
    : ResiliencePolicyBase,
    IRedisResiliencePolicy
{
    // Constructors
    public RedisResiliencePolicy(
        ILogger<RedisResiliencePolicy> logger
    ) : base(logger)
    {
    }
}