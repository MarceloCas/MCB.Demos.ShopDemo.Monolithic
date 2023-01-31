using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;
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
    public async Task<bool> ExecuteAsync(Func<(bool OpenTransaction, bool IsBulkInsertOperation, CancellationToken CancellationToken), Task<bool>> handler, bool openTransaction, bool isBulkInsertOperation,  CancellationToken cancellationToken)
    {
        _entityFrameworkDataContext.SetIsBulkInsertOperation(isBulkInsertOperation);

        var result = await handler((openTransaction, isBulkInsertOperation, cancellationToken));

        if (result)
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
        else
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);

        return result;
    }

    public async Task<bool> ExecuteAsync<TInput>(Func<(TInput? Input, bool OpenTransaction, bool IsBulkInsertOperation, CancellationToken CancellationToken), Task<bool>> handler, TInput? input, bool openTransaction, bool isBulkInsertOperation, CancellationToken cancellationToken)
    {
        _entityFrameworkDataContext.SetIsBulkInsertOperation(isBulkInsertOperation);

        var result = await handler((input, openTransaction, isBulkInsertOperation, cancellationToken));

        if (result)
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
        else
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);

        return result;
    }

    public async Task<(bool Success, TOutput? Output)> ExecuteAsync<TInput, TOutput>(Func<(TInput? Input, bool OpenTransaction, bool IsBulkInsertOperation, CancellationToken CancellationToken), Task<(bool Success, TOutput? Output)>> handler, TInput? input, bool openTransaction, bool isBulkInsertOperation, CancellationToken cancellationToken)
    {
        _entityFrameworkDataContext.SetIsBulkInsertOperation(isBulkInsertOperation);

        var result = await handler((input, openTransaction, isBulkInsertOperation, cancellationToken));

        if (result.Success)
            await _entityFrameworkDataContext.CommitTransactionAsync(cancellationToken);
        else
            await _entityFrameworkDataContext.RollbackTransactionAsync(cancellationToken);

        return result;
    }
}
