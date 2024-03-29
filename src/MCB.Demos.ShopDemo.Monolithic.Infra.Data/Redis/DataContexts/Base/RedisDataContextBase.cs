﻿using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Models;
using StackExchange.Redis;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base;

public abstract class RedisDataContextBase
    : IRedisDataContext
{
    // Constants
    public const string CONNECTION_CLOSED = "CONNECTION_CLOSED";

    // Fields
    private readonly RedisOptions _redisOptions;
    private ITransaction? _currentTransaction;
    private bool disposedValue;

    // Properties
    public ConnectionMultiplexer? ConnectionMultiplexer { get; private set; }
    protected bool IsConnected => ConnectionMultiplexer?.IsConnected == true;
    protected IDatabase? Database { get; private set; }

    // Constructors
    protected RedisDataContextBase(RedisOptions redisOptions)
    {
        _redisOptions = redisOptions;
    }

    // Private Methods
    private void ValidateConnection()
    {
        if (!IsConnected)
            throw new InvalidOperationException(CONNECTION_CLOSED);
    }
    private void TryOpenConnectionInternal()
    {
        if (IsConnected)
            return;

        ConnectionMultiplexer = ConnectionMultiplexer.Connect(_redisOptions.ConnectionString);

        Database = ConnectionMultiplexer.GetDatabase();
    }

    // Public Methods
    public virtual Task TryOpenConnectionAsync(CancellationToken cancellationToken)
    {
        TryOpenConnectionInternal();

        return Task.CompletedTask;
    }
    public virtual async Task CloseConnectionAsync(CancellationToken cancellationToken)
    {
        if (!IsConnected)
            return;

        await ConnectionMultiplexer!.CloseAsync(allowCommandsToComplete: true);
    }

    public virtual Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        ValidateConnection();

        _currentTransaction = Database!.CreateTransaction();

        return Task.CompletedTask;
    }
    public virtual async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        ValidateConnection();

        if (_currentTransaction == null)
            return;

        await _currentTransaction.ExecuteAsync();

        _currentTransaction = null;
    }
    public virtual Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = null;

        return Task.CompletedTask;
    }

    public virtual Task<RedisValue> StringGetAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        TryOpenConnectionInternal();

        return Database!.StringGetAsync(key, flags: commandFlags);
    }
    public virtual Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry, CommandFlags commandFlags = CommandFlags.None)
    {
        TryOpenConnectionInternal();

        return _currentTransaction is null
            ? Database!.StringSetAsync(key, value, expiry, flags: commandFlags)
            : _currentTransaction.StringSetAsync(key, value, expiry, flags: commandFlags);
    }
    public virtual Task<double> StringIncrementAsync(string key, double value = 1, CommandFlags commandFlags = CommandFlags.None)
    {
        TryOpenConnectionInternal();

        return _currentTransaction is null
            ? Database!.StringIncrementAsync(key, value, flags: commandFlags)
            : _currentTransaction.StringIncrementAsync(key, value, flags: commandFlags);
    }
    public virtual Task<bool> RemoveAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        TryOpenConnectionInternal();

        return _currentTransaction is null
            ? Database!.KeyDeleteAsync(key, flags: commandFlags)
            : _currentTransaction.KeyDeleteAsync(key, flags: commandFlags);
    }

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            disposedValue = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    } 
    #endregion
}