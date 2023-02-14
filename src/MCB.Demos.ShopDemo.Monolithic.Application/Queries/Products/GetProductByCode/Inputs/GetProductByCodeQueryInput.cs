using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Inputs;
public record GetProductByCodeQueryInput
    : QueryInputBase
{
    // Properties
    public string Code { get; }

    // Constructors
    public GetProductByCodeQueryInput(
        Guid correlationId,
        Guid tenantId,
        string executionUser,
        string sourcePlatform,
        string code
    ) : base(correlationId, tenantId, executionUser, sourcePlatform)
    {
        Code = code;
    }
}

