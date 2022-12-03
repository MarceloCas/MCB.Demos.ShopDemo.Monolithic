namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

public abstract class ResponseBase
{
    public string? ExecutionUser { get; set; }
    public string? SourcePlatform { get; set; }
    public IEnumerable<ResponseMessage> ResponseMessageCollection { get; set; }

    protected ResponseBase()
    {
        ResponseMessageCollection = Enumerable.Empty<ResponseMessage>();
    }
}
public abstract class ResponseBase<TData>
    : ResponseBase
{
    public TData? Data { get; set; }
}