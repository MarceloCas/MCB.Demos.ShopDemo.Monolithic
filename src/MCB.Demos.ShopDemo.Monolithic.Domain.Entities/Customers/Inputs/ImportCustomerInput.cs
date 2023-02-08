using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;

public sealed record ImportCustomerInput
    : InputBase
{
    // Properties
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime BirthDate { get; }
    public string Email { get; }

    // Constructors
    public ImportCustomerInput(
        Guid tenantId,
        string firstName,
        string lastName,
        DateTime birthDate,
        string email,
        string executionUser,
        string sourcePlatform,
        Guid correlationId
    ) : base(tenantId, executionUser, sourcePlatform, correlationId)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Email = email;
    }
}