using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Responses;

public class ValidateImportProductBatchResponse
    : ResponseBase
{
    // Properties
    public IEnumerable<ValidateImportProductBatchResponseItem>? ItemCollection { get; set; }
    public bool Success { get; set; }
}
