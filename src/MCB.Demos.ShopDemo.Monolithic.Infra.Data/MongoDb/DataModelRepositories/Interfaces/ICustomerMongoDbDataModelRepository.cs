using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModelRepositories.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModelRepositories.Interfaces;

public interface ICustomerMongoDbDataModelRepository
    : IMongoDbDataModelRepository<CustomerMongoDbDataModel>
{
}