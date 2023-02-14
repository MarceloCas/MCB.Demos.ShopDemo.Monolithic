using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Base.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Interfaces;
public interface IProductDataModelRedisRepository
    : IRedisDataModelRepository<ProductDataModel>
{
    string GetKey(Guid tenantId, string code);
}
