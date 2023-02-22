using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

public class RemoveCustomerPayload
    : PayloadBase
{
    public string? Email { get; set; }
}
