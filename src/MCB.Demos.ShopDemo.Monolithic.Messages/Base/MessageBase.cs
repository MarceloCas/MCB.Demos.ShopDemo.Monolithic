namespace MCB.Demos.ShopDemo.Monolithic.Messages.Base;

public abstract record MessageBase
{
    // Properties
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public DateTime Timestamp { get; set; }
    public string ExecutionUser { get; set; }
    public string SourcePlatform { get; set; }

    protected MessageBase()
    {
        ExecutionUser = string.Empty;
        SourcePlatform = string.Empty;
    }
}