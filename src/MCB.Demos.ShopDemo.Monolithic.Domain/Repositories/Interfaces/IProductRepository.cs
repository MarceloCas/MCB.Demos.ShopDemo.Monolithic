using MCB.Core.Domain.Abstractions.Repositories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
public interface IProductRepository
    : IRepository<Product>
{
    Task<Product?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken);
    Task<(bool Success, int ModifiedCount)> ImportProductAsync(Product product, CancellationToken cancellationToken);
    Task<bool> RemoveProductAsync(Product product, CancellationToken cancellationToken);
}