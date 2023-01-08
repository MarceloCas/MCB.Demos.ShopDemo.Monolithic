using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;

public interface IEntityFrameworkDataContext
    : IDataContext
{
    DbSet<TDataModel> GetDbSet<TDataModel>() where TDataModel : DataModelBase;
    EntityEntry<TDataModel> SetEntry<TDataModel>(TDataModel dataModel) where TDataModel : DataModelBase;
}
