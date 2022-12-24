using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Interfaces;

public interface IEntityFrameworkDataContext
    : IDataContext
{
    DbSet<TDataModel> GetDbSet<TDataModel>() where TDataModel : DataModelBase;
    EntityEntry<TDataModel> SetEntry<TDataModel>(TDataModel dataModel) where TDataModel : DataModelBase;
}
