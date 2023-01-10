using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;

public record SetExistingCustomerInfoInput
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
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }

    // Constructors
    public SetExistingCustomerInfoInput(
        Guid tenantId,
        Guid id,
        string createdBy,
        DateTime createdAt,
        string? lastUpdatedBy,
        DateTime? lastUpdatedAt,
        string lastSourcePlatform,
        DateTime registryVersion,
        string firstName,
        string lastName,
        DateTime birthDate,
        string email,
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
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Email = email;
    }
}