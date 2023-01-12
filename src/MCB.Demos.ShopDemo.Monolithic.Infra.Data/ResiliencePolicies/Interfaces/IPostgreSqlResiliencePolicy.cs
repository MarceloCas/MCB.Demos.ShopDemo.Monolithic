using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;

public interface IPostgreSqlResiliencePolicy
    : IResiliencePolicy
{
}
