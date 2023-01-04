using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail;

public record GetCustomerByEmailQueryInput
    : QueryInputBase
{
    // Properties
    public string Email { get; }

    // Constructors
    public GetCustomerByEmailQueryInput(
        Guid correlationId,
        Guid tenantId,
        string executionUser,
        string sourcePlatform,
        string email
    ) : base(correlationId, tenantId, executionUser, sourcePlatform)
    {
        Email = email;
    }
}
