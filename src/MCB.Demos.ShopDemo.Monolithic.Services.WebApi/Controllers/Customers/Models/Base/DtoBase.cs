namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Models.Base;

public abstract class DtoBase
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string LastSourcePlatform { get; set; } = null!;
    public DateTime RegistryVersion { get; set; }
}
