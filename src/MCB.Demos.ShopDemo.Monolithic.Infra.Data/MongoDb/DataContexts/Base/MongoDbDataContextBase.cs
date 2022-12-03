using MongoDB.Driver;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base;

public abstract class MongoDbDataContextBase
    : IMongoDbDataContext
{
    // Constants
    public const string TRANSACTION_ALREADY_STARTED = "TRANSACTION_ALREADY_STARTED";
    public const string TRANSACTION_NOT_STARTED = "TRANSATRANSACTION_NOT_STARTEDCTION_ALREADY_STARTED";
    public const string COLLECTION_NOT_FOUNDED = "COLLECTION_NOT_FOUNDED";

    // Fields
    private IClientSessionHandle? _clientSessionHandle;
    private static Dictionary<Type, object> _mongoCollectionDictionary = new();

    // Static Fields
    private static bool _hasInitialized;

    // Properties
    protected MongoDbOptions Options { get; }
    protected MongoClient Client { get; }
    protected IMongoDatabase? Database { get; private set; }

    // Constructors
    protected MongoDbDataContextBase(
        MongoClient client,
        MongoDbOptions options
    )
    {
        Client = client;
        Options = options;

        Database = Client.GetDatabase(
            name: Options.DatabaseName,
            settings: Options.MongoDatabaseSettings
        );
    }

    // Public Methods
    public Task OpenConnectionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task CloseConnectionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_clientSessionHandle is not null)
            throw new InvalidOperationException(TRANSACTION_ALREADY_STARTED);

        _clientSessionHandle = await Client.StartSessionAsync(
            options: Options.MongoDbClientSessionOptions,
            cancellationToken
        );
    }
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_clientSessionHandle is null)
            throw new InvalidOperationException(TRANSACTION_NOT_STARTED);

        await _clientSessionHandle.CommitTransactionAsync(cancellationToken);

        _clientSessionHandle.Dispose();
        _clientSessionHandle = null;
    }
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_clientSessionHandle is null)
            throw new InvalidOperationException(TRANSACTION_NOT_STARTED);

        await _clientSessionHandle.AbortTransactionAsync(cancellationToken);

        _clientSessionHandle.Dispose();
        _clientSessionHandle = null;
    }

    public IMongoCollection<TMongoDbDataModel> GetCollection<TMongoDbDataModel>()
    {
        _mongoCollectionDictionary.TryGetValue(typeof(TMongoDbDataModel), out object? mongoCollection);

        return mongoCollection is null
            ? throw new InvalidOperationException(COLLECTION_NOT_FOUNDED)
            : (IMongoCollection<TMongoDbDataModel>)mongoCollection;
    }

    public void RegisterMongoCollection<TMongoDbDataModel>(
        string name,
        IMongoDbDataModelMap<TMongoDbDataModel> mongoDbDataModelMap,
        Func<IndexKeysDefinition<TMongoDbDataModel>> indexConfigHandler,
        MongoCollectionSettings? collectionSettings = null
    )
        where TMongoDbDataModel : MongoDbDataModelBase
    {
        // Map
        mongoDbDataModelMap.Map();

        // Indexes
        //var indexKeysDefinition = Builders<TMongoDbDataModel>.IndexKeys
        var indexKeysDefinition = indexConfigHandler()
            .Ascending(q => q.CreatedBy)
            .Ascending(q => q.CreatedAt)
            .Ascending(q => q.LastUpdatedAt)
            .Ascending(q => q.LastUpdatedBy)
            .Ascending(q => q.LastSourcePlatform)
            .Ascending(q => q.RegistryVersion);

        var mongoCollection = Database!.GetCollection<TMongoDbDataModel>(name, collectionSettings);

        mongoCollection.Indexes.CreateOne(
            model: new CreateIndexModel<TMongoDbDataModel>(
                indexKeysDefinition,
                options: new CreateIndexOptions
                {
                    Sparse = true
                }
            )
        );

        // Register
        _mongoCollectionDictionary.Add(
            key: typeof(TMongoDbDataModel),
            value: mongoCollection
        );
    }

    public void Init()
    {
        if (_hasInitialized)
            return;

        new MongoDbDataModelBaseMap().Map();
        InitInternal();

        _hasInitialized = true;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    // Protected Absctract Methods
    protected abstract void InitInternal();
}