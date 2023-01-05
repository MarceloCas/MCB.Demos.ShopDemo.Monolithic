using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data;

public class DefaultUnitOfWork
    : IUnitOfWork
{
    // Fields
    private readonly IEntityFrameworkDataContext _entityFrameworkDataContext;
    private readonly IRedisDataContext _redisDataContext;

    // Constructors
    public DefaultUnitOfWork(
        IEntityFrameworkDataContext entityFrameworkDataContext,
        IRedisDataContext redisDataContext
    )
    {
        _entityFrameworkDataContext = entityFrameworkDataContext;
        _redisDataContext = redisDataContext;
    }

    // Public Methods
    public async Task<bool> ExecuteAsync(Func<(bool OpenTransaction, CancellationToken CancellationToken), Task<bool>> handler, bool openTransaction, CancellationToken cancellationToken)
    {
        var result = await handler((openTransaction, cancellationToken));

        if (result)
        {
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
            await _redisDataContext.CommitTransactionAsync(cancellationToken);
        }
        else
        {
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);
            await _redisDataContext.RollbackTransactionAsync(cancellationToken);
        }

        return result;
    }

    public async Task<bool> ExecuteAsync<TInput>(Func<(TInput? Input, bool OpenTransaction, CancellationToken CancellationToken), Task<bool>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {
        var result = await handler((input, openTransaction, cancellationToken));

        if (result)
        {
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
            await _redisDataContext.CommitTransactionAsync(cancellationToken);
        }
        else
        {
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);
            await _redisDataContext.RollbackTransactionAsync(cancellationToken);
        }

        return result;
    }

    public async Task<(bool Success, TOutput? Output)> ExecuteAsync<TInput, TOutput>(Func<(TInput? Input, bool OpenTransaction, CancellationToken CancellationToken), Task<(bool Success, TOutput? Output)>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {
        var result = await handler((input, openTransaction, cancellationToken));

        if (result.Success)
        {
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
            await _redisDataContext.CommitTransactionAsync(cancellationToken);
        }
        else
        {
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);
            await _redisDataContext.RollbackTransactionAsync(cancellationToken);
        }

        return result;
    }
}
