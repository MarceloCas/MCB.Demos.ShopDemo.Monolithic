using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings.Base;

public interface IMongoDbDataModelMap<TMongoDbDataModel>
    where TMongoDbDataModel : MongoDbDataModelBase
{
    void Map();
}