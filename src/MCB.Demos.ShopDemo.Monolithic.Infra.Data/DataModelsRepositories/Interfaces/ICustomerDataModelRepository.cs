using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Interfaces;
public interface ICustomerDataModelRepository
    : IDataModelRepository<CustomerDataModel>
{
    Task<CustomerDataModel?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken);
}
