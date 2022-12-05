using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Enums;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

public struct ResponseMessage
{
    public ResponseMessageType Type { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

}