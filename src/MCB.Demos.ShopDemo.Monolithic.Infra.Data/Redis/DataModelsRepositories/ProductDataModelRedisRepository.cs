using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories;
public class ProductDataModelRedisRepository
    : RedisDataModelRepositoryBase<ProductDataModel>,
    IProductDataModelRedisRepository
{
    // Constructors
    public ProductDataModelRedisRepository(
        ITraceManager traceManager,
        IRedisDataContext redisDataContext,
        IJsonSerializer jsonSerializer
    ) : base(traceManager, redisDataContext, jsonSerializer)
    {

    }

    // Private Methods
    public string GetKey(Guid tenantId, string code)
    {
        return $"mcb|product|{tenantId}|{code}";
    }

    // Protected Methods
    public override string? GetKey(ProductDataModel? dataModel)
    {
        return dataModel is null ? null : GetKey(dataModel.TenantId, dataModel.Code);
    }
}
