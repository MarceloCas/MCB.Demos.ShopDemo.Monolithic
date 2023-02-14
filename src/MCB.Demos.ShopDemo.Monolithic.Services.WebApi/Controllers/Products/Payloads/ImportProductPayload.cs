using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Payloads;

public class ImportProductPayload
    : PayloadBase
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}