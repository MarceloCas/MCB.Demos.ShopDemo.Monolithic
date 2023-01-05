using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories;

public class CustomerDataModelEntityFrameworkRepository
    : EntityFrameworkDataModelRepositoryBase<CustomerDataModel>,
    ICustomerDataModelRepository
{
    public CustomerDataModelEntityFrameworkRepository(
        IEntityFrameworkDataContext entityFrameworkDataContext
    ) : base(entityFrameworkDataContext)
    {
    }

    // Public Methods
    public async Task<CustomerDataModel?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken)
    {
        return (
            await GetAsync(q => q.TenantId == tenantId && q.Email == email, cancellationToken)
        ).FirstOrDefault();
    }
}
