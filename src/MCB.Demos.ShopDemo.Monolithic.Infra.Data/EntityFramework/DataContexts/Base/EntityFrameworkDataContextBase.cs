using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base;
public abstract class EntityFrameworkDataContextBase
    : DbContext,
    IEntityFrameworkDataContext
{
    // Properties
    protected string ConnectionString { get; }

    // Constructors
    protected EntityFrameworkDataContextBase(string connectionString)
    {
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
    public Task OpenConnectionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task CloseConnectionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
