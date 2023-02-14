using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;
public sealed record ImportProductInput
    : InputBase
{
    // Properties
    public string Code { get; }
    public string Description { get; }

    // Constructors
    public ImportProductInput(
        Guid tenantId,
        string code,
        string description,
        string executionUser,
        string sourcePlatform,
        Guid correlationId
    ) : base(tenantId, executionUser, sourcePlatform, correlationId)
    {
        Code = code;
        Description = description;
    }
}
