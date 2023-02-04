namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RedisConfig;

public class Redis
{
    // Properties
    public string ConnectionString { get; set; } = null!;
    public TtlSeconds TtlSeconds { get; set; } = null!;
    public ResiliencePolicy ResiliencePolicy { get; set; } = null!;

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        var typeFullName = typeof(Redis).FullName;

        if (string.IsNullOrEmpty(ConnectionString))
            messageCollection.Add($"{typeFullName}.{nameof(ConnectionString)} cannot be null");

        if (TtlSeconds is null)
            messageCollection.Add($"{typeFullName}.{nameof(TtlSeconds)} cannot be null");
        else
            messageCollection.AddRange(TtlSeconds.Validate().Messages);

        if (ResiliencePolicy is null)
            messageCollection.Add($"{typeFullName}.{nameof(ResiliencePolicy)} cannot be null");
        else
            messageCollection.AddRange(ResiliencePolicy.Validate().Messages);

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}