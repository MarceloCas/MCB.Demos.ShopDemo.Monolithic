using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

public class ImportCustomerBatchPayload
	: PayloadBase
{
    public ImportCustomerBatchPayloadItem[] Items { get; set; }

	public ImportCustomerBatchPayload()
	{
		Items = Array.Empty<ImportCustomerBatchPayloadItem>();
	}
}
