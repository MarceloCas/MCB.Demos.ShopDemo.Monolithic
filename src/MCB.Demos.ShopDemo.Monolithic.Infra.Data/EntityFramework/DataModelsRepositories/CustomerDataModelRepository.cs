using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories;

public class CustomerDataModelRepository
    : DataModelRepositoryBase<CustomerDataModel>,
    ICustomerDataModelRepository
{
    public CustomerDataModelRepository(
        IEntityFrameworkDataContext entityFrameworkDataContext
    ) : base(entityFrameworkDataContext)
    {
    }
}
