using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Payloads;

public class ImportProductBatchPayload
    : PayloadBase
{
    public ImportProductBatchPayloadItem[] Items { get; set; }

    public ImportProductBatchPayload()
    {
        Items = Array.Empty<ImportProductBatchPayloadItem>();
    }
}
