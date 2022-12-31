using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;

public record ImportCustomerServiceInput
    : ServiceInputBase
{
    // Properties
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime BirthDate { get; }
    public string Email { get; }

    public ImportCustomerServiceInput(
        Guid tenantId,
        string firstName,
        string lastName,
        DateTime birthDate,
        string email,
        string executionUser,
        string sourcePlatform
    ) : base(tenantId, executionUser, sourcePlatform)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Email = email;
    }
}