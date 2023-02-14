using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Interfaces;
public interface IGetProductByCodeQuery
    : IQuery<GetProductByCodeQueryInput, Product?>
{

}
