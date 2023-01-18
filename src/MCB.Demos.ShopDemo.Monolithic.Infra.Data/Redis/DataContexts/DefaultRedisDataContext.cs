using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using StackExchange.Redis;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts;

public class DefaultRedisDataContext
    : RedisDataContextBase,
    IRedisDataContext
{
    // Fields
    private readonly IRedisResiliencePolicy _redisResiliencePolicy;

    // Constructors
    public DefaultRedisDataContext(
        RedisOptions redisOptions,
        IRedisResiliencePolicy redisResiliencePolicy
    ) : base(redisOptions)
    {
        _redisResiliencePolicy = redisResiliencePolicy;
    }

    // Public Methods
    public override Task TryOpenConnectionAsync(CancellationToken cancellationToken)
    {
        return _redisResiliencePolicy.ExecuteAsync(
            handler: base.TryOpenConnectionAsync,
            cancellationToken
        );
    }
    public override Task CloseConnectionAsync(CancellationToken cancellationToken)
    {
        return _redisResiliencePolicy.ExecuteAsync(
            handler: base.CloseConnectionAsync,
            cancellationToken
        );
    }

    public override Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return _redisResiliencePolicy.ExecuteAsync(
            handler: base.BeginTransactionAsync,
            cancellationToken
        );
    }
    public override Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return _redisResiliencePolicy.ExecuteAsync(
            handler: base.CommitTransactionAsync,
            cancellationToken
        );
    }

    public override async Task<RedisValue> StringGetAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        var result = await _redisResiliencePolicy.ExecuteAsync(
            handler: (input, cancellationToken) => base.StringGetAsync(input.Key, input.CommandFlags),
            input: (Key: key, CommandFlags: commandFlags),
            cancellationToken: default
        );

        return result.Output;
    }
    public override async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry, CommandFlags commandFlags = CommandFlags.None)
    {
        var result = await _redisResiliencePolicy.ExecuteAsync(
            handler: (input, cancellationToken) => base.StringSetAsync(input.Key, input.Value, input.Expiry, input.CommandFlags),
            input: (Key: key, Value: value, Expiry: expiry, CommandFlags: commandFlags),
            cancellationToken: default
        );

        return result.Success && result.Output;
    }
    public override async Task<double> StringIncrementAsync(string key, double value = 1, CommandFlags commandFlags = CommandFlags.None)
    {
        var result = await _redisResiliencePolicy.ExecuteAsync(
            handler: (input, cancellationToken) => base.StringIncrementAsync(input.Key, input.Value, input.CommandFlags),
            input: (Key: key, Value: value, CommandFlags: commandFlags),
            cancellationToken: default
        );

        return result.Output;
    }
    public override async Task<bool> RemoveAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        var result = await _redisResiliencePolicy.ExecuteAsync(
            handler: (input, cancellationToken) => base.RemoveAsync(input.Key, input.CommandFlags),
            input: (Key: key, CommandFlags: commandFlags),
            cancellationToken: default
        );

        return result.Output;
    }
}