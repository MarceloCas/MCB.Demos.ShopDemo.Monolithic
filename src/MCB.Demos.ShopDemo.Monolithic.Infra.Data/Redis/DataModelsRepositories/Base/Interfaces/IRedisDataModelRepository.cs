using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using StackExchange.Redis;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Base.Interfaces;
public interface IRedisDataModelRepository<TDataModel>
    where TDataModel : DataModelBase
{
    string? GetKey(TDataModel? dataModel);

    Task<bool> AddOrUpdateAsync(TDataModel dataModel, TimeSpan? expiry, CancellationToken cancellationToken);
    Task<TDataModel?> GetAsync(string key, CommandFlags commandFlags = CommandFlags.None);
    Task<bool> RemoveAsync(TDataModel dataModel, CancellationToken cancellationToken);
}
