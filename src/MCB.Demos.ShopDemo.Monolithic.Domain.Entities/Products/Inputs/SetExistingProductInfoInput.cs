using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;
public record SetExistingProductInfoInput
    : InputBase
{
    // Properties
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string LastSourcePlatform { get; set; }
    public DateTime RegistryVersion { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    // Constructors
    public SetExistingProductInfoInput(
        Guid tenantId,
        Guid id,
        string createdBy,
        DateTime createdAt,
        string? lastUpdatedBy,
        DateTime? lastUpdatedAt,
        string lastSourcePlatform,
        DateTime registryVersion,
        string code,
        string description,
        string executionUser,
        string sourcePlatform,
        Guid correlationId
    ) : base(tenantId, executionUser, sourcePlatform, correlationId)
    {
        Id = id;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        LastUpdatedBy = lastUpdatedBy;
        LastUpdatedAt = lastUpdatedAt;
        LastSourcePlatform = lastSourcePlatform;
        RegistryVersion = registryVersion;
        Code = code;
        Description = description;
    }
}
