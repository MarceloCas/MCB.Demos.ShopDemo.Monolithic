using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Inputs;
public record ImportProductUseCaseInput
    : UseCaseInputBase
{
    // Properties
    public string? Code { get; set; }
    public string? Description { get; set; }
}
