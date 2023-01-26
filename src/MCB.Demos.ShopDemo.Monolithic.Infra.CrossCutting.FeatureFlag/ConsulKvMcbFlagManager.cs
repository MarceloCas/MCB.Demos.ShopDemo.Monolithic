using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
using Consul;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag;
public class ConsulKvMcbFlagManager
    : IMcbFeatureFlagManager
{
    // Constants
    public const string FAIL_ON_REFRESH_FEATURE_FLAGS_MESSAGE = "Fail on refresh feature flags";

    // Fields
    private readonly ILogger<ConsulKvMcbFlagManager> _logger;
    private readonly IKVEndpoint _kvEndpoint;
    private readonly Dictionary<string, bool> _featureFlagsDictionary;
    private readonly int _refreshIntervalInSeconds;

    // Constructors
    public ConsulKvMcbFlagManager(
        ILogger<ConsulKvMcbFlagManager> logger,
        AppSettings appSettings
    )
    {
        _logger = logger;

        var consultClient = new ConsulClient(
            config: new ConsulClientConfiguration
            {
                Address = new Uri(appSettings.Consul.Address)
            }
        );
        
        _kvEndpoint = consultClient.KV;

        _refreshIntervalInSeconds = appSettings.Consul.RefreshIntervalInSeconds;

        _featureFlagsDictionary = new Dictionary<string, bool>();
    }
    // Private Methods
    private static string GetKey(Guid tenantId, string? executionUser, string key) => $"feature-flags/tenants/{tenantId}{(string.IsNullOrEmpty(executionUser) ? string.Empty : ($"/{executionUser}"))}/{key}";
    private async Task<QueryResult<KVPair[]>?> GetFeatureFlagcollectionAsync(CancellationToken cancellationToken)
    {
        var queryResult = default(QueryResult<KVPair[]>);

        try
        {
            queryResult = await _kvEndpoint.List(prefix: "feature-flags/", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(exception: ex, message: FAIL_ON_REFRESH_FEATURE_FLAGS_MESSAGE);
        }

        return queryResult;
    }
    private static bool GetKvPairValue(KVPair? kvPair)
    {
        if (kvPair is null)
            return false;

        var stringValue = Encoding.UTF8.GetString(kvPair.Value);
        if (stringValue is null)
            return false;

        _ = bool.TryParse(stringValue, out var value);

        return value;
    }
    private async Task RefreshFeatureFlags(CancellationToken cancellationToken)
    {
        var queryResult = await GetFeatureFlagcollectionAsync(cancellationToken);

        if (queryResult is null)
            return;

        foreach (var kvPair in queryResult.Response)
        {
            var value = GetKvPairValue(kvPair);

            if (!_featureFlagsDictionary.ContainsKey(kvPair.Key))
                _featureFlagsDictionary.Add(kvPair.Key, value);
            else
                _featureFlagsDictionary[kvPair.Key] = value;
        }
    }
    private void StartRefreshFeatureFlagsTask(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
            {
                while (true)
                {
                    await RefreshFeatureFlags(cancellationToken);
                    await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), cancellationToken);
                }
            },
            cancellationToken
        );
    }

    // Public Methods
    public async Task InitAsync(CancellationToken cancellationToken)
    {
        await RefreshFeatureFlags(cancellationToken);
        StartRefreshFeatureFlagsTask(cancellationToken);
    }

    public bool GetFlag(string key)
    {
        _ = _featureFlagsDictionary.TryGetValue(key, out bool value);

        return value;
    }
    public bool GetFlag(Guid tenantId, string? executionUser, string key)
    {
        return GetFlag(GetKey(tenantId, executionUser, key));
    }

    public Task<bool> GetFlagAsync(string key, CancellationToken cancellationToken)
    {
        return Task.FromResult(GetFlag(key));
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
