namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Abstractions;

public interface IDataContext
    : IDisposable
{
    Task OpenConnectionAsync(CancellationToken cancellationToken);
    Task CloseConnectionAsync(CancellationToken cancellationToken);

    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}