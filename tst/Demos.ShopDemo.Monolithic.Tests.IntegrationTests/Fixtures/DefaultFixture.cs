using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

namespace Demos.ShopDemo.Monolithic.Tests.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(DefaultFixture))]
public class DefaultFixtureCollection
    : ICollectionFixture<DefaultFixtureCollection>
{

}
public class DefaultFixture
{
    // Properties
    public Guid TenantId { get; }
    public string ExecutionUser { get; }
    public string SourcePlatform { get; }

    // Constructors
    public DefaultFixture()
    {
        TenantId = Guid.NewGuid();
        ExecutionUser = "marcelo.castelo";
        SourcePlatform = "IntegrationTests";
    }

    // Public Static Methods
    public ImportCustomerPayload GenerateNewImportCustomerPayload(
        Guid? tenantId = null,
        string? executionUser = null,
        string? sourcePlatform = null,
        string? firstName = null,
        string? lastName = null,
        DateTime? birthDate = null,
        string? email = null
    )
    {
        return new ImportCustomerPayload
        {
            TenantId = tenantId ?? TenantId,
            ExecutionUser = executionUser ?? ExecutionUser,
            SourcePlatform = sourcePlatform ?? SourcePlatform,
            FirstName = firstName ?? Guid.NewGuid().ToString(),
            LastName = lastName ?? Guid.NewGuid().ToString(),
            BirthDate = birthDate ?? DateTime.UtcNow.AddYears(-21),
            Email = email ?? $"{Guid.NewGuid()}@mcb.com"
        };
    }
}
