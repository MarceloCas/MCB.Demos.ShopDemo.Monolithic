using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base;
public abstract class EntityFrameworkDataModelRepositoryBase<TDataModel>
    : IDataModelRepository<TDataModel>
    where TDataModel : DataModelBase
{
    // Fields
    private readonly IEntityFrameworkDataContext _entityFrameworkDataContext;

    // Properties
    protected DbSet<TDataModel> DbSet { get; }

    // Constructors
    protected EntityFrameworkDataModelRepositoryBase(
        IEntityFrameworkDataContext entityFrameworkDataContext
    )
    {
        _entityFrameworkDataContext = entityFrameworkDataContext;
        DbSet = entityFrameworkDataContext.GetDbSet<TDataModel>();
    }

    // Public Methods
    public ValueTask<EntityEntry<TDataModel>> AddAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        return DbSet.AddAsync(dataModel, cancellationToken);
    }
    public ValueTask<EntityEntry<TDataModel>> UpdateAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        var entry = _entityFrameworkDataContext.SetEntry(dataModel);
        entry.CurrentValues.SetValues(dataModel);

        return ValueTask.FromResult(entry);
    }
    public async ValueTask<EntityEntry<TDataModel>> AddOrUpdateAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        var existingDataModel = await DbSet.FindAsync(new object?[] { dataModel.Id }, cancellationToken);

        return existingDataModel == null
            ? await DbSet.AddAsync(dataModel, cancellationToken)
            : await UpdateAsync(dataModel, cancellationToken);
    }

    public ValueTask<EntityEntry<TDataModel>> RemoveAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(DbSet.Remove(dataModel));
    }
    public Task<(bool success, int removeCount)> RemoveAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<(bool success, int removeCount)> RemoveAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TDataModel> Get(Func<TDataModel, bool> expression)
    {
        throw new NotImplementedException();
    }
    public Task<TDataModel> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<TDataModel>> GetAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(expression);
        var localResult = DbSet.Local.Where(expression);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id);

        return Task.FromResult(merged);
    }
    public IEnumerable<TDataModel> GetAll(Guid tenantId)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(q => q.TenantId == tenantId);
        var localResult = DbSet.Local.Where(q => q.TenantId == tenantId);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id);

        return merged;
    }
    public Task<IEnumerable<TDataModel>> GetAllAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(q => q.TenantId == tenantId);
        var localResult = DbSet.Local.Where(q => q.TenantId == tenantId);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id).AsEnumerable();

        return Task.FromResult(merged);
    }
}
