using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data.Common;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;

public interface IEntityFrameworkDataContext
    : IDataContext
{
    DbConnection GetDbConnection();
    DbSet<TDataModel> GetDbSet<TDataModel>() where TDataModel : DataModelBase;
    EntityEntry<TDataModel> SetEntry<TDataModel>(TDataModel dataModel) where TDataModel : DataModelBase;

    IEntityType? GetEntityType(Type type);
    IEntityType? GetEntityType<T>();
}
