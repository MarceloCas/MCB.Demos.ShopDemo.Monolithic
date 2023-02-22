using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.RemoveProduct.Inputs;
public record RemoveProductUseCaseInput
    : UseCaseInputBase
{
    // Properties
    public string? Code { get; set; }
}
