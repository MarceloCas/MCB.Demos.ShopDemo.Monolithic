using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data;
public class DefaultUnitOfWork
    : IUnitOfWork
{
    // Fields
    private readonly IDefaultMongoDbDataContext _dataContext;

    // Constructors
    public DefaultUnitOfWork(
        IDefaultMongoDbDataContext dataContext
    )
    {
        _dataContext = dataContext;
    }

    // Public Methods
    public async Task<bool> ExecuteAsync(Func<(bool openTransaction, CancellationToken cancellationToken), Task<bool>> handler, bool openTransaction, CancellationToken cancellationToken)
    {
        var hasStartedTransaction = false;

        try
        {
            if (openTransaction)
            {
                await _dataContext.BeginTransactionAsync(cancellationToken);
                hasStartedTransaction = true;
            }

            var processResult = await handler((openTransaction, cancellationToken));

            if(openTransaction)
            {
                if(processResult)
                    await _dataContext.CommitTransactionAsync(cancellationToken);
                else
                    await _dataContext.RollbackTransactionAsync(cancellationToken);
            }

            return processResult;
        }
        catch (Exception)
        {
            if(hasStartedTransaction)
                await _dataContext.RollbackTransactionAsync(cancellationToken);

            return false;
        }
    }

    public async Task<bool> ExecuteAsync<TInput>(Func<(TInput? input, bool openTransaction, CancellationToken cancellationToken), Task<bool>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {
        var hasStartedTransaction = false;

        try
        {
            if (openTransaction)
            {
                await _dataContext.BeginTransactionAsync(cancellationToken);
                hasStartedTransaction = true;
            }

            var processResult = await handler((input, openTransaction, cancellationToken));

            if (openTransaction)
            {
                if (processResult)
                    await _dataContext.CommitTransactionAsync(cancellationToken);
                else
                    await _dataContext.RollbackTransactionAsync(cancellationToken);
            }

            return processResult;
        }
        catch (Exception)
        {
            if (hasStartedTransaction)
                await _dataContext.RollbackTransactionAsync(cancellationToken);

            return false;
        }
    }

    public async Task<(bool success, TOutput? output)> ExecuteAsync<TInput, TOutput>(Func<(TInput? input, bool openTransaction, CancellationToken cancellationToken), Task<(bool Success, TOutput? Output)>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {
        var hasStartedTransaction = false;

        try
        {
            if (openTransaction)
            {
                await _dataContext.BeginTransactionAsync(cancellationToken);
                hasStartedTransaction = true;
            }

            var processResult = await handler((input, openTransaction, cancellationToken));

            if (openTransaction)
            {
                if (processResult.Success)
                    await _dataContext.CommitTransactionAsync(cancellationToken);
                else
                    await _dataContext.RollbackTransactionAsync(cancellationToken);
            }

            return processResult;
        }
        catch (Exception)
        {
            if (hasStartedTransaction)
                await _dataContext.RollbackTransactionAsync(cancellationToken);

            return default;
        }
    }
}
