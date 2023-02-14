using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Responses;

public class ImportProductResponse
    : ResponseBase
{
    // Properties
    public IEnumerable<ProductDto>? ProductCollection { get; set; }
}