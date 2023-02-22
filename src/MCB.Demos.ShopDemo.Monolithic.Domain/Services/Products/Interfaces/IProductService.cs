using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Interfaces;
public interface IProductService
    : IService<Product>
{
    Task<(bool Success, Product? ImportedProduct)> ImportProductAsync(ImportProductServiceInput input, CancellationToken cancellationToken);
    Task<(bool Success, IEnumerable<Notification>? NotificationCollection)> ValidateImportProductAsync(ValidateImportProductServiceInput input, CancellationToken cancellationToken);
    Task<(bool Success, Product? RemovedProduct)> RemoveProductAsync(RemoveProductServiceInput input, CancellationToken cancellationToken);
}
