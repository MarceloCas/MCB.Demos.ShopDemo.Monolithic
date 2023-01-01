namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

public abstract class PayloadBase
{
    public Guid CorrelationId { get; set; }
    public Guid TenantId { get; set; }
    public string? ExecutionUser { get; set; }
    public string? SourcePlatform { get; set; }
}