using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

public class ValidateImportCustomerBatchPayload
    : PayloadBase
{
    public ValidateImportCustomerBatchPayloadItem[] Items { get; set; }

    public ValidateImportCustomerBatchPayload()
    {
        Items = Array.Empty<ValidateImportCustomerBatchPayloadItem>();
    }
}
