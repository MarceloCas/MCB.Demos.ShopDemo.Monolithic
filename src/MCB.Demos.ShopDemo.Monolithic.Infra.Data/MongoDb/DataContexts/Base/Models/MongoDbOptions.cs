using MongoDB.Driver;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base.Models;

public class MongoDbOptions
{
    public string ConnectionString { get; }
    public string DatabaseName { get; }
    public MongoDatabaseSettings? MongoDatabaseSettings { get; }
    public ClientSessionOptions? MongoDbClientSessionOptions { get; }

    public MongoDbOptions(
        string connectionString,
        string databaseName,
        MongoDatabaseSettings? mongoDatabaseSettings,
        ClientSessionOptions? mongoDbClientSessionOptions
    )
    {
        ConnectionString = connectionString;
        DatabaseName = databaseName;
        MongoDatabaseSettings = mongoDatabaseSettings;
        MongoDbClientSessionOptions = mongoDbClientSessionOptions;
    }
}