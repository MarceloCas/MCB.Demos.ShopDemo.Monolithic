﻿using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
public record RemoveCustomerServiceInput
    : ServiceInputBase
{
    // Properties
    public string Email { get; }

    // Constructors
    public RemoveCustomerServiceInput(
        Guid correlationId,
        Guid tenantId,
        string email,
        string executionUser,
        string sourcePlatform
    ) : base(correlationId, tenantId, executionUser, sourcePlatform)
    {
        Email = email;
    }
}
