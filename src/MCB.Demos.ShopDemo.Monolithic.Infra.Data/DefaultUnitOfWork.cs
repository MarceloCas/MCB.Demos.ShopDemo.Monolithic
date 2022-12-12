using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data;
public class DefaultUnitOfWork
    : IUnitOfWork
{
    // Fields

    // Constructors
    public DefaultUnitOfWork(
    )
    {
    }

    // Public Methods
    public Task<bool> ExecuteAsync(Func<(bool openTransaction, CancellationToken cancellationToken), Task<bool>> handler, bool openTransaction, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public Task<bool> ExecuteAsync<TInput>(Func<(TInput? input, bool openTransaction, CancellationToken cancellationToken), Task<bool>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {

        return Task.FromResult(true);
    }

    public Task<(bool success, TOutput? output)> ExecuteAsync<TInput, TOutput>(Func<(TInput? input, bool openTransaction, CancellationToken cancellationToken), Task<(bool Success, TOutput? Output)>> handler, TInput? input, bool openTransaction, CancellationToken cancellationToken)
    {
        return Task.FromResult(default((bool success, TOutput? output)));
    }
}
