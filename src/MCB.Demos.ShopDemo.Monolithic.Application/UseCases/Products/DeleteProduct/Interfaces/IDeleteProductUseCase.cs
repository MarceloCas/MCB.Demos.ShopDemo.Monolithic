using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct.Interfaces;
public interface IDeleteProductUseCase
    : IUseCase<DeleteProductUseCaseInput, Product>
{
}
