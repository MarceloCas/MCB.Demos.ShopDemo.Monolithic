using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
    public DbSet<TDataModel> GetDbSet<TDataModel>() where TDataModel : DataModelBase
    {
        return Set<TDataModel>();
    }
    public EntityEntry<TDataModel> SetEntry<TDataModel>(TDataModel dataModel) where TDataModel : DataModelBase
    {
        return Entry(dataModel);
    }
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
