using System.Text.Json.Serialization;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

public abstract class ResponseBase
{
    [JsonPropertyName("messages")]
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