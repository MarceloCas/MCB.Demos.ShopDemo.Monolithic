namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

public class ValidateImportCustomerBatchPayloadItem
{
    private DateTime _birthDate;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate
    {
        get { return _birthDate; }
        set { _birthDate = DateTime.SpecifyKind(value, DateTimeKind.Utc).Date; }
    }
    public string? Email { get; set; }
}
