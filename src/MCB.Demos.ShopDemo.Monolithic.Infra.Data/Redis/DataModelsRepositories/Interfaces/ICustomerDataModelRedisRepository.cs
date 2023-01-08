using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Base.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Interfaces;

public interface ICustomerDataModelRedisRepository
    : IRedisDataModelRepository<CustomerDataModel>
{
    string GetKey(Guid tenantId, string email);
}
