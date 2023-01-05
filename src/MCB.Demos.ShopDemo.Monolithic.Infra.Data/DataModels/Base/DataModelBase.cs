namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;

public abstract class DataModelBase
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string LastSourcePlatform { get; set; } = null!;
    public DateTime RegistryVersion { get; set; }
}
