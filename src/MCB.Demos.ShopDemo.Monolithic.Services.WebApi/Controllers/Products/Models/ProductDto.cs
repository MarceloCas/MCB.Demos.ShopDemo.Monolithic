using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Models;

public class ProductDto
    : DtoBase
{
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
}
