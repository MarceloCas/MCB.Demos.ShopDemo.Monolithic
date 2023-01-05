using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Models;
using StackExchange.Redis;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base;

public abstract class RedisDataContextBase
    : IRedisDataContext
{
    // Constants
    public const string CONNECTION_CLOSED = "CONNECTION_CLOSED";

    // Fields
    private readonly RedisOptions _redisOptions;
    private ConnectionMultiplexer? _connectionMultiplexer;
    private ITransaction? _currentTransaction;

    // Properties
    protected bool IsConnected => _connectionMultiplexer?.IsConnected == true;
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
    private void ProcessTransaction()
    {
        _connectionMultiplexer!.Wait(_currentTransaction!.ExecuteAsync());
        _currentTransaction = null;
    }

    // Public Methods
    public async Task OpenConnectionAsync(CancellationToken cancellationToken)
    {
        if (IsConnected)
            return;

        _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_redisOptions.ConnectionString);

        Database = _connectionMultiplexer.GetDatabase();
    }
    public async Task CloseConnectionAsync(CancellationToken cancellationToken)
    {
        if (!IsConnected)
            return;

        await _connectionMultiplexer!.CloseAsync(allowCommandsToComplete: true);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        ValidateConnection();

        _currentTransaction = Database!.CreateTransaction();

        return Task.CompletedTask;
    }
    public Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        ValidateConnection();

        ProcessTransaction();

        return Task.CompletedTask;
    }
    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        ValidateConnection();

        // Force false condition to rollback
        var tempGuid = Guid.NewGuid().ToString();
        _currentTransaction!.AddCondition(Condition.StringEqual(key: tempGuid, value: tempGuid));

        ProcessTransaction();

        return Task.CompletedTask;
    }

    public Task<RedisValue> StringGetAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        ValidateConnection();

        return Database!.StringGetAsync(key, flags: commandFlags);
    }
    public Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry, CommandFlags commandFlags = CommandFlags.None)
    {
        ValidateConnection();

        return _currentTransaction is null
            ? Task.FromResult(Database!.StringSet(key, value, expiry, flags: commandFlags))
            : _currentTransaction.StringSetAsync(key, value, expiry, flags: commandFlags);
    }
    public Task<double> StringIncrementAsync(string key, double value = 1, CommandFlags commandFlags = CommandFlags.None)
    {
        ValidateConnection();

        return _currentTransaction is null
            ? Task.FromResult(Database!.StringIncrement(key, value, flags: commandFlags))
            : _currentTransaction.StringIncrementAsync(key, value, flags: commandFlags);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

}