namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

public abstract class ResponseBase
{
    public IEnumerable<ResponseMessage>? Messages { get; set; }
}
public abstract class ResponseBase<TData>
    : ResponseBase
{
    public TData? Data { get; set; }
}