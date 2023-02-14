using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Factories.Interfaces;
public interface IProductFactory
    : IFactory<Product>
{

}
