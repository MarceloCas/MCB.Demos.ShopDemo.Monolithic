using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModelRepositories.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels.Base;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModelRepositories.Base;

public abstract class MongoDbDataModelRepositoryBase<TMongoDbDataModel>
    : IMongoDbDataModelRepository<TMongoDbDataModel>
    where TMongoDbDataModel : MongoDbDataModelBase
{
    // Protected Properties
    protected IMongoDbDataContext DataContext { get; }
    protected IMongoCollection<TMongoDbDataModel> Collection { get; }

    // Constructors
    protected MongoDbDataModelRepositoryBase(
        IMongoDbDataContext dataContext
    )
    {
        DataContext = dataContext;
        Collection = DataContext.GetCollection<TMongoDbDataModel>();
    }

    // Public Methods
    public Task AddAsync(TMongoDbDataModel dataModel, CancellationToken cancellationToken)
    {
        return Collection.InsertOneAsync(
            dataModel,
            options: null,
            cancellationToken: cancellationToken
        );
    }
    public async Task<IEnumerable<TMongoDbDataModel>> FindAsync(Expression<Func<TMongoDbDataModel, bool>> filter, CancellationToken cancellationToken)
    {
        return (
            await Collection.FindAsync(
                filter: filter,
                options: null,
                cancellationToken
            )
        ).ToEnumerable(cancellationToken);
    }
}