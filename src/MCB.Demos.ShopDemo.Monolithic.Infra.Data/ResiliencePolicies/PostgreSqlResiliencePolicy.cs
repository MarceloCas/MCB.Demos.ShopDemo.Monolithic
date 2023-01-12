using MCB.Core.Infra.CrossCutting.DesignPatterns.Resilience;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using Microsoft.Extensions.Logging;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies;
public class PostgreSqlResiliencePolicy
    : ResiliencePolicyBase,
    IPostgreSqlResiliencePolicy
{
    // Constructors
    public PostgreSqlResiliencePolicy(
        ILogger<PostgreSqlResiliencePolicy> logger
    ) : base(logger)
    {
    }
}
