using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Payloads;

public class ValidateImportProductBatchPayload
    : PayloadBase
{
    public ValidateImportProductBatchPayloadItem[] Items { get; set; }

    public ValidateImportProductBatchPayload()
    {
        Items = Array.Empty<ValidateImportProductBatchPayloadItem>();
    }
}
