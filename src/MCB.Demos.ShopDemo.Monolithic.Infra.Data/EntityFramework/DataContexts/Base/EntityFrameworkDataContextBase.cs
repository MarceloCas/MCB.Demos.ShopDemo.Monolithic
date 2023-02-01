using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data.Common;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base;
public abstract class EntityFrameworkDataContextBase
    : DbContext,
    IEntityFrameworkDataContext
{
    // Constants
    public const string COMMIT_TRANSACTION_TRACE_NAME = $"{nameof(EntityFrameworkDataContextBase)}.{nameof(CommitTransactionAsync)}";

    // Properties
    protected ITraceManager TraceManager { get; }
    protected string ConnectionString { get; }
    public bool IsBulkInsertOperation { get; private set; }

    // Constructors
    protected EntityFrameworkDataContextBase(
        ITraceManager traceManager,
        string connectionString
    )
    {
        TraceManager = traceManager;
        ConnectionString = connectionString;
    }

    // Protected Methods
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
        OnConfiguringInternal(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        OnModelCreatingInternal(modelBuilder);
    }

    // Protected Abstract Methods
    protected abstract void OnConfiguringInternal(DbContextOptionsBuilder optionsBuilder);
    protected abstract void OnModelCreatingInternal(ModelBuilder modelBuilder);

    // Public Methods
    public DbConnection GetDbConnection()
    {
        return Database.GetDbConnection();
    }
    public virtual DbSet<TDataModel> GetDbSet<TDataModel>() where TDataModel : DataModelBase
    {
        return Set<TDataModel>();
    }
    public virtual EntityEntry<TDataModel> SetEntry<TDataModel>(TDataModel dataModel) where TDataModel : DataModelBase
    {
        return Entry(dataModel);
    }
    public virtual Task TryOpenConnectionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public virtual Task CloseConnectionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public virtual Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public virtual Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: COMMIT_TRANSACTION_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: Guid.Empty,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: default(object),
            handler: async (input, activity, cancellationToken) =>
            {
                await SaveChangesAsync(cancellationToken);
            },
            cancellationToken
        )!;
    }
    public virtual Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public IEntityType? GetEntityType(Type type)
    {
        return Model.FindEntityType(type);
    }
    public IEntityType? GetEntityType<T>()
    {
        return Model.FindEntityType(typeof(T));
    }

    public void SetIsBulkInsertOperation(bool isBulkInsertOperation)
    {
        IsBulkInsertOperation = isBulkInsertOperation;
    }
}
