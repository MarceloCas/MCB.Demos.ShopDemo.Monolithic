namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

public class ImportCustomerBatchPayload
{
    public ImportCustomerBatchPayloadItem[] Items { get; set; }

	public ImportCustomerBatchPayload()
	{
		Items = Array.Empty<ImportCustomerBatchPayloadItem>();
	}
}
