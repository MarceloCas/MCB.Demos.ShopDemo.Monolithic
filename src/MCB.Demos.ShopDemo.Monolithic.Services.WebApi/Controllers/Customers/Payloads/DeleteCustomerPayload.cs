using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

public class DeleteCustomerPayload
    : PayloadBase
{
    public string? Email { get; set; }
}
