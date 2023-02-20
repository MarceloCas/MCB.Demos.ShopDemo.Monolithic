using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
public record SetExistingOrderItemInfoInput
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
    public int Sequence { get; }
    public string? Description { get; }
    public decimal Quantity { get; }
    public decimal UnityValue { get; }
    public Product Product { get; } = null!;

    // Constructors
    public SetExistingOrderItemInfoInput(
        Guid tenantId,
        Guid id,
        string createdBy,
        DateTime createdAt,
        string? lastUpdatedBy,
        DateTime? lastUpdatedAt,
        string lastSourcePlatform,
        DateTime registryVersion,
        int sequence,
        string? description,
        decimal quantity,
        decimal unityValue,
        Product product,
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
        Sequence = sequence;
        Description = description;
        Quantity = quantity;
        UnityValue = unityValue;
        Product = product;
        Description = description;
    }
}
