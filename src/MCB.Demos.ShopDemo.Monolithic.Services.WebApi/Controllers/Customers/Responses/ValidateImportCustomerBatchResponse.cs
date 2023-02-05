using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;

public class ValidateImportCustomerBatchResponse
    : ResponseBase
{
    // Properties
    public IEnumerable<ValidateImportCustomerBatchResponseItem> ItemCollection { get; set; }
    public bool Success { get; set; }

    // Constructors
    public ValidateImportCustomerBatchResponse()
    {
        ItemCollection = Enumerable.Empty<ValidateImportCustomerBatchResponseItem>();
    }
    public ValidateImportCustomerBatchResponse(IEnumerable<ValidateImportCustomerBatchResponseItem> itemCollection)
    {
        ItemCollection = itemCollection;
        Success = !itemCollection.Any(q => !q.Success);
    }
}
