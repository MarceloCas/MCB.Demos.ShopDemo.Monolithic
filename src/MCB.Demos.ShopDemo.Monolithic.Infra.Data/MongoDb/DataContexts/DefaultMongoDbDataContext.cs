using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings;
using MongoDB.Driver;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts;

public class DefaultMongoDbDataContext
    : MongoDbDataContextBase,
    IDefaultMongoDbDataContext
{
    // Constructors
    public DefaultMongoDbDataContext(
        MongoClient client,
        MongoDbOptions options
    ) : base(client, options)
    {

    }

    // Protected Methods
    protected override void InitInternal()
    {
        RegisterMongoCollection(
            name: "Customers",
            mongoDbDataModelMap: new CustomerMongoDbDataModelMap(),
            indexConfigHandler: () =>
            {
                return Builders<CustomerMongoDbDataModel>.IndexKeys
                    .Ascending(q => q.Email)
                    .Ascending(q => q.BirthDate);
            }
        );
    }
}