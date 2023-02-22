using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.RemoveProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.RemoveProduct.Interfaces;
public interface IRemoveProductUseCase
    : IUseCase<RemoveProductUseCaseInput, Product>
{
}
