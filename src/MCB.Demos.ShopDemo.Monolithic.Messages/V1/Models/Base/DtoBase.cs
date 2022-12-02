namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;

public class DtoBase
{
    // Properties
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string? LastSourcePlatform { get; set; }
    public DateTime RegistryVersion { get; set; }

    // Constructors
    protected DtoBase()
    {
        CreatedBy = string.Empty;
    }
}