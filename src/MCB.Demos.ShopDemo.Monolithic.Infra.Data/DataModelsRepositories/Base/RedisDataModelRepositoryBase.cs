﻿using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base;

public abstract class RedisDataModelRepositoryBase<TDataModel>
    : IDataModelRepository<TDataModel>
    where TDataModel : DataModelBase
{
    // Properties
    protected IRedisDataContext RedisDataContext { get; }
    protected EntityFrameworkDataModelRepositoryBase<TDataModel> EntityFrameworkDataModelRepository { get; }
    protected IJsonSerializer JsonSerializer { get; }

    // Constructors
    protected RedisDataModelRepositoryBase(
        IRedisDataContext redisDataContext,
        EntityFrameworkDataModelRepositoryBase<TDataModel> entityFrameworkDataModelRepository,
        IJsonSerializer jsonSerializer
    )
    {
        RedisDataContext = redisDataContext;
        EntityFrameworkDataModelRepository = entityFrameworkDataModelRepository;
        JsonSerializer = jsonSerializer;
    }

    // Protected Methods
    protected abstract string? GetKey(TDataModel? dataModel);

    // Public Methods
    public virtual async ValueTask<EntityEntry<TDataModel>> AddAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        await RedisDataContext.TryOpenConnectionAsync(cancellationToken);
        var key = GetKey(dataModel);

        if (key is null)
            return default;

        await RedisDataContext.StringSetAsync(
            key,
            value: JsonSerializer.SerializeToJson(dataModel),
            expiry: TimeSpan.FromMinutes(30)
        );

        return default;
    }
    public virtual ValueTask<EntityEntry<TDataModel>> AddOrUpdateAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.AddOrUpdateAsync(dataModel, cancellationToken);
    }

    public virtual ValueTask<EntityEntry<TDataModel>> UpdateAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.UpdateAsync(dataModel, cancellationToken);
    }

    public virtual ValueTask<EntityEntry<TDataModel>> RemoveAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.RemoveAsync(dataModel, cancellationToken);
    }
    public virtual Task<(bool success, int removeCount)> RemoveAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.RemoveAsync(expression, cancellationToken);
    }
    public virtual Task<(bool success, int removeCount)> RemoveAllAsync(CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.RemoveAllAsync(cancellationToken);
    }

    public virtual IEnumerable<TDataModel> Get(Func<TDataModel, bool> expression)
    {
        return EntityFrameworkDataModelRepository.Get(expression);
    }
    public virtual Task<TDataModel> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.GetAsync(tenantId, id, cancellationToken);
    }
    public virtual Task<IEnumerable<TDataModel>> GetAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.GetAsync(expression, cancellationToken);
    }
    public virtual IEnumerable<TDataModel> GetAll(Guid tenantId)
    {
        return EntityFrameworkDataModelRepository.GetAll(tenantId);
    }
    public virtual Task<IEnumerable<TDataModel>> GetAllAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        return EntityFrameworkDataModelRepository.GetAllAsync(tenantId, cancellationToken);
    }
}
