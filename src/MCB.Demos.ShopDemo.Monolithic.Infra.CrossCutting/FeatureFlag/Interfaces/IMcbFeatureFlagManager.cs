using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.FeatureFlags;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
public interface IMcbFeatureFlagManager
    : IFeatureFlagManager
{
    Task InitAsync(CancellationToken cancellationToken);

    bool GetFlag(Guid tenantId, string? executionUser, string key);
    Task<bool> GetFlagAsync(Guid tenantId, string? executionUser, string key, CancellationToken cancellationToken);

    void AddOrUpdateFlag(Guid tenantId, string? executionUser, string key, bool value);
    Task AddOrUpdateFlagAsync(Guid tenantId, string? executionUser, string key, bool value, CancellationToken cancellationToken);

    void RemoveFlag(Guid tenantId, string? executionUser, string key);
    Task RemoveFlagAsync(Guid tenantId, string? executionUser, string key, CancellationToken cancellationToken);
}
