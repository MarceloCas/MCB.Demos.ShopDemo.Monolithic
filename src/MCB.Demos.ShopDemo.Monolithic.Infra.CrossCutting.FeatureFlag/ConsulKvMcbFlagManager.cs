using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
using Consul;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag;
public class ConsulKvMcbFlagManager
    : IMcbFeatureFlagManager
{
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

    // Public Methods
    public Task InitAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
            {
                while (true)
                {
                    var queryResult = default(QueryResult<KVPair[]>);

                    try
                    {
                        queryResult = await _kvEndpoint.List(prefix: "feature-flags/", cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(exception: ex, message: null);
                    }

                    if (queryResult is null)
                        continue;

                    foreach (var kvPair in queryResult.Response)
                    {
                        var stringValue = Encoding.UTF8.GetString(kvPair.Value);
                        if (stringValue is null)
                            return false;

                        _ = bool.TryParse(stringValue, out var value);

                        if(!_featureFlagsDictionary.ContainsKey(kvPair.Key))
                            _featureFlagsDictionary.Add(kvPair.Key, value);
                        else
                            _featureFlagsDictionary[kvPair.Key] = value;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds));
                }
            }, 
            cancellationToken
        );

        return Task.CompletedTask;
    }

    public bool GetFlag(string key)
    {
        if(_featureFlagsDictionary.TryGetValue(key, out bool value)) 
            return value;

        var kvPair = _kvEndpoint.Get(key, ct: default).GetAwaiter().GetResult().Response;
        if(kvPair is null) 
            return false;

        var stringValue = Encoding.UTF8.GetString(kvPair.Value);
        if (stringValue is null)
            return false;

        _ = bool.TryParse(stringValue, out value);

        return value;
    }
    public bool GetFlag(Guid tenantId, string? executionUser, string key)
    {
        return GetFlag(GetKey(tenantId, executionUser, key));
    }

    public async Task<bool> GetFlagAsync(string key, CancellationToken cancellationToken)
    {
        if (_featureFlagsDictionary.TryGetValue(key, out bool value))
            return value;

        var kvPair = (await _kvEndpoint.Get(key, cancellationToken)).Response;
        if (kvPair is null)
            return false;

        var stringValue = Encoding.UTF8.GetString(kvPair.Value);
        if (stringValue is null)
            return false;

        _ = bool.TryParse(stringValue, out value);

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
