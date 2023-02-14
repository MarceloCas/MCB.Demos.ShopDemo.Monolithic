using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;

public class ImportCustomerResponse
    : ResponseBase
{
    // Properties
    public IEnumerable<CustomerDto>? CustomerCollection { get; set; }
}