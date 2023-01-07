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
    private async Task ProcessTransactionAsync()
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.ExecuteAsync();

        _currentTransaction = null;
    }

    // Public Methods
    public Task TryOpenConnectionAsync(CancellationToken cancellationToken)
    {
        if (IsConnected)
            return Task.CompletedTask;

        _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisOptions.ConnectionString);

        Database = _connectionMultiplexer.GetDatabase();

        return Task.CompletedTask;
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
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        ValidateConnection();

        await ProcessTransactionAsync();
    }
    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = null;

        return Task.CompletedTask;
    }

    public async Task<RedisValue> StringGetAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        ValidateConnection();

        return await Database!.StringGetAsync(key, flags: commandFlags);
    }
    public Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry, CommandFlags commandFlags = CommandFlags.None)
    {
        ValidateConnection();

        return _currentTransaction is null
            ? Database!.StringSetAsync(key, value, expiry, flags: commandFlags)
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