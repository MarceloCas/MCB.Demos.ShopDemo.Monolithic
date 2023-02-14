using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;
public record ImportProductServiceInput
    : ServiceInputBase
{
    // Properties
    public string Code { get; }
    public string Description { get; }

    public ImportProductServiceInput(
        Guid correlationId,
        Guid tenantId,
        string code,
        string description,
        string executionUser,
        string sourcePlatform
    ) : base(correlationId, tenantId, executionUser, sourcePlatform)
    {
        Code = code;
        Description = description;
    }
}