namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

public abstract class DtoBase
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string LastSourcePlatform { get; set; } = null!;
    public Guid LastCorrelationId { get; set; }
    public DateTime RegistryVersion { get; set; }
}
