using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
public interface IProductDataModelEntityFrameworkRepository
    : IEntityFrameworkDataModelRepository<ProductDataModel>
{
    Task<ProductDataModel?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken);
}

