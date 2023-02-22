using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;
public record RemoveProductServiceInput
    : ServiceInputBase
{
    // Properties
    public string Code { get; }

    // Constructors
    public RemoveProductServiceInput(
        Guid correlationId,
        Guid tenantId,
        string code,
        string executionUser,
        string sourcePlatform
    ) : base(correlationId, tenantId, executionUser, sourcePlatform)
    {
        Code = code;
    }
}
