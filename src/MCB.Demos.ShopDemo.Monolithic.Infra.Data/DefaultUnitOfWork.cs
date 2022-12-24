using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data;
public class DefaultUnitOfWork
    : IUnitOfWork
{
    // Fields
    private readonly IEntityFrameworkDataContext _entityFrameworkDataContext;

    // Constructors
    public DefaultUnitOfWork(
        IEntityFrameworkDataContext entityFrameworkDataContext
    )
    {
        _entityFrameworkDataContext = entityFrameworkDataContext;
    }

    // Public Methods
    public async Task<bool> ExecuteAsync(Func<(bool openTransaction, CancellationToken cancellationToken), Task<bool>> handler, bool openTransaction, CancellationToken cancellationToken)
    {
        var result = await handler((openTransaction, cancellationToken));

        if (result)
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
        else
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);

        return result;
    }

    public async Task<bool> ExecuteAsync<TInput>(Func<(TInput? input, bool openTransaction, CancellationToken cancellationToken), Task<bool>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {
        var result = await handler((input, openTransaction, cancellationToken));

        if (result)
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
        else
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);

        return result;
    }

    public async Task<(bool success, TOutput? output)> ExecuteAsync<TInput, TOutput>(Func<(TInput? input, bool openTransaction, CancellationToken cancellationToken), Task<(bool Success, TOutput? Output)>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {
        var result = await handler((input, openTransaction, cancellationToken));

        if (result.Success)
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
        else
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);

        return result;
    }
}
