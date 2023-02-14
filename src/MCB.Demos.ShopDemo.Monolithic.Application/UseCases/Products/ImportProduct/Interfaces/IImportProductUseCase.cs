using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Interfaces;
public interface IImportProductUseCase
    : IUseCase<ImportProductUseCaseInput, Product>
{
}