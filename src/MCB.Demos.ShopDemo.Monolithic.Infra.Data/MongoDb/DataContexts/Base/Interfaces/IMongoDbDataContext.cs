using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Abstractions;
using MongoDB.Driver;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base.Interfaces;

public interface IMongoDbDataContext
    : IDataContext
{
    IMongoCollection<TMongoDbDataModel> GetCollection<TMongoDbDataModel>();
}