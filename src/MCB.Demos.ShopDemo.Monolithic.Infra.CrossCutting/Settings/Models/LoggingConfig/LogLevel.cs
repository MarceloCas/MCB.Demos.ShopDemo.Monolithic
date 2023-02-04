namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.LoggingConfig;
public class LogLevel
{
    // Properties
    public string Default { get; set; } = null!;

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        if (string.IsNullOrWhiteSpace(Default))
            messageCollection.Add($"{typeof(LogLevel).FullName}.{nameof(Default)} cannot be null");

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
