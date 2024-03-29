﻿namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;

public interface IDataContext
    : IDisposable
{
    Task TryOpenConnectionAsync(CancellationToken cancellationToken);
    Task CloseConnectionAsync(CancellationToken cancellationToken);

    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}