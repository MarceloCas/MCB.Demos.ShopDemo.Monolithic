using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
using Consul;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using System.Text;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag;
public class ConsulKvMcbFlagManager
    : IMcbFeatureFlagManager
{
    // Fields
    private readonly IKVEndpoint _kvEndpoint;

    // Constructors
    public ConsulKvMcbFlagManager(
        AppSettings appSettings
    )
    {
        var consultClient = new ConsulClient(
            config: new ConsulClientConfiguration
            {
                Address = new Uri(appSettings.Consul.Address)
            }
        );
        _kvEndpoint = consultClient.KV;
    }
    // Private Methods
    private static string GetKey(Guid tenantId, string? executionUser, string key) => $"feature-flags/tenants/{tenantId}{(string.IsNullOrEmpty(executionUser) ? string.Empty : ($"/{executionUser}"))}/{key}";

    // Public Methods
    public Task InitAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;    
    }

    public bool GetFlag(string key)
    {
        var kvPair = _kvEndpoint.Get(key, ct: default).GetAwaiter().GetResult().Response;
        if(kvPair is null) 
            return false;

        var stringValue = Encoding.UTF8.GetString(kvPair.Value);
        if (stringValue is null)
            return false;

        _ = bool.TryParse(stringValue, out var value);

        return value;
    }
    public bool GetFlag(Guid tenantId, string? executionUser, string key)
    {
        return GetFlag(GetKey(tenantId, executionUser, key));
    }

    public async Task<bool> GetFlagAsync(string key, CancellationToken cancellationToken)
    {
        var kvPair = (await _kvEndpoint.Get(key, cancellationToken)).Response;
        if (kvPair is null)
            return false;

        var stringValue = Encoding.UTF8.GetString(kvPair.Value);
        if (stringValue is null)
            return false;

        _ = bool.TryParse(stringValue, out var value);

        return value;
    }
    public Task<bool> GetFlagAsync(Guid tenantId, string? executionUser, string key, CancellationToken cancellationToken)
    {
        return GetFlagAsync(GetKey(tenantId, executionUser, key), cancellationToken);
    }

    public void AddOrUpdateFlag(string key, bool value)
    {
        throw new NotImplementedException();
    }
    public void AddOrUpdateFlag(Guid tenantId, string? executionUser, string key, bool value)
    {
        throw new NotImplementedException();
    }

    public Task AddOrUpdateFlagAsync(string key, bool value, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task AddOrUpdateFlagAsync(Guid tenantId, string? executionUser, string key, bool value, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void RemoveFlag(string key)
    {
        throw new NotImplementedException();
    }
    public void RemoveFlag(Guid tenantId, string? executionUser, string key)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFlagAsync(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task RemoveFlagAsync(Guid tenantId, string? executionUser, string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
