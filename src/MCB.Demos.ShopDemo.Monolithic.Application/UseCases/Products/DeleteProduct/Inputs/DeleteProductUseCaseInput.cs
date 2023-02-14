using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct.Inputs;
public record DeleteProductUseCaseInput
    : UseCaseInputBase
{
    // Properties
    public string? Code { get; set; }
}
