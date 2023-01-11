namespace MCB.Demos.ShopDemo.Monolithic.Messages.Base;

public abstract class MessageBase
{
    // Properties
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public DateTime Timestamp { get; set; }
    public string ExecutionUser { get; set; }
    public string SourcePlatform { get; set; }
    public Guid CorrelationId { get; set; }

    protected MessageBase()
    {
        ExecutionUser = string.Empty;
        SourcePlatform = string.Empty;
    }
}