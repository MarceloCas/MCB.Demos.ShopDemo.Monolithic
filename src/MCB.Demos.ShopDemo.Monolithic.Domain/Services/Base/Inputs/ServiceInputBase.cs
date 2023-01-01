namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Inputs;

public abstract record ServiceInputBase
{
    // Properties
    public Guid CorrelationId { get; set; }
    public Guid TenantId { get; }
    public string ExecutionUser { get; }
    public string SourcePlatform { get; }

    // Constructors
    protected ServiceInputBase(Guid correlationId, Guid tenantId, string executionUser, string sourcePlatform)
    {
        CorrelationId = correlationId;
        TenantId = tenantId;
        ExecutionUser = executionUser;
        SourcePlatform = sourcePlatform;
    }
}