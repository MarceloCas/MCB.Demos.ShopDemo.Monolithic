namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels.Base;

public abstract class MongoDbDataModelBase
{
    // Properties
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string LastSourcePlatform { get; set; }
    public DateTime RegistryVersion { get; set; }

    public IDictionary<string, object>? ExtraElements { get; set; }

    // Public Methods
    public MongoDbDataModelBase()
    {
        CreatedBy = string.Empty;
        LastSourcePlatform = string.Empty;
    }
}