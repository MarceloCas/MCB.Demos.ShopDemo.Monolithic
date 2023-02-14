using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;
public class ProductDto
    : DtoBase
{
    // Properties
    public string Code { get; set; }
    public string Description { get; set; }

    // Constructors
    public ProductDto()
        : base()
    {
        Code = Description = string.Empty;
    }
}