namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base;

public abstract record QueryInputBase
{
    // Properties
    public Guid CorrelationId { get; }
    public Guid TenantId { get; }
    public string ExecutionUser { get; }
    public string SourcePlatform { get; }

    // Constructors
    protected QueryInputBase(Guid correlationId, Guid tenantId, string executionUser, string sourcePlatform)
    {
        CorrelationId = correlationId;
        TenantId = tenantId;
        ExecutionUser = executionUser;
        SourcePlatform = sourcePlatform;
    }
}
