namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.PostgreSqlConfig;

public class PostgreSql
{
    // Properties
    public string ConnectionString { get; set; } = null!;
    public ResiliencePolicy ResiliencePolicy { get; set; } = null!;

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        var typeFullName = typeof(PostgreSql).FullName;

        if (string.IsNullOrWhiteSpace(ConnectionString))
            messageCollection.Add($"{typeFullName}.{nameof(ConnectionString)} cannot be null");

        if (ResiliencePolicy is null)
            messageCollection.Add($"{typeFullName}.{nameof(ResiliencePolicy)} cannot be null");
        else
            messageCollection.AddRange(ResiliencePolicy.Validate().Messages);

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}