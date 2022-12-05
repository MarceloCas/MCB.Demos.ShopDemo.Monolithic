using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

public class ImportCustomerPayload
    : PayloadBase
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