using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
public record ImportOrderItemInput
    : InputBase
{
    // Properties
    public int Sequence { get; }
    public string? Description { get; }
    public decimal Quantity { get; }
    public decimal UnityValue { get; }
    public Product Product { get; } = null!;

    // Constructors
    public ImportOrderItemInput(
        Guid tenantId,
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
        Sequence = sequence;
        Description = description;
        Quantity = quantity;
        UnityValue = unityValue;
        Product = product;
    }
}
