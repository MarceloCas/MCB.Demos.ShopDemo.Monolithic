﻿using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base.Interfaces;

public interface IDataModelRepository<TDataModel> 
    where TDataModel : DataModelBase
{
    ValueTask<EntityEntry<TDataModel>> AddAsync(TDataModel dataModel, CancellationToken cancellationToken);

    ValueTask<EntityEntry<TDataModel>> UpdateAsync(TDataModel dataModel, CancellationToken cancellationToken);

    ValueTask<EntityEntry<TDataModel>> AddOrUpdateAsync(TDataModel dataModel, CancellationToken cancellationToken);

    ValueTask<EntityEntry<TDataModel>> RemoveAsync(TDataModel dataModel, CancellationToken cancellationToken);

    Task<(bool success, int removeCount)> RemoveAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken);

    Task<(bool success, int removeCount)> RemoveAllAsync(CancellationToken cancellationToken);

    Task<TDataModel> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<TDataModel>> GetAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken);

    Task<IEnumerable<TDataModel>> GetAllAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

    IEnumerable<TDataModel> Get(Func<TDataModel, bool> expression);

    IEnumerable<TDataModel> GetAll(Guid tenantId, Guid id);
}
