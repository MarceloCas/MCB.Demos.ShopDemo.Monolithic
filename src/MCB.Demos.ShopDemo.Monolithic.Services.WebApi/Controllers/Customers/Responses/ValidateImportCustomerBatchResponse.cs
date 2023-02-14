using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;

public class ValidateImportCustomerBatchResponse
    : ResponseBase
{
    // Properties
    public IEnumerable<ValidateImportCustomerBatchResponseItem>? ItemCollection { get; set; }
    public bool Success { get; set; }
}
