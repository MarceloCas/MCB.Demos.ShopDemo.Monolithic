namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;
public class OpenTelemetry
{
    // Properties
    public string GrpcCollectorReceiverUrl { get; set; } = null!;
    public bool EnableConsoleExporter { get; set; }

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        if (string.IsNullOrEmpty(GrpcCollectorReceiverUrl))
            messageCollection.Add($"{typeof(OpenTelemetry).FullName}.{nameof(GrpcCollectorReceiverUrl)} cannot be null");

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
