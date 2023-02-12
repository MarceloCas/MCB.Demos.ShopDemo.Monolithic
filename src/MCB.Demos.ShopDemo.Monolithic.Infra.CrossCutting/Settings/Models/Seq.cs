namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

public class Seq
{
    // Properties
    public string Url { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string MinimumLevel { get; set; } = null!;
    public bool UseSeqInsteadOfOpenTelemetryExport { get; set; }

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        if (string.IsNullOrEmpty(Url))
            messageCollection.Add($"{typeof(Consul).FullName}.{nameof(Url)} cannot be null");

        if (string.IsNullOrEmpty(ApiKey))
            messageCollection.Add($"{typeof(Consul).FullName}.{nameof(ApiKey)} cannot be null");

        if (string.IsNullOrEmpty(MinimumLevel))
            messageCollection.Add($"{typeof(Consul).FullName}.{nameof(MinimumLevel)} cannot be null");

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
