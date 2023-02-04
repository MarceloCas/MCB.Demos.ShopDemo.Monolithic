namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.LoggingConfig;
public class Logging
{
    // Properties
    public LogLevel LogLevel { get; set; } = null!;

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        if (LogLevel is null)
            messageCollection.Add($"{typeof(Logging).FullName}.{nameof(LogLevel)} cannot be null");
        else
            messageCollection.AddRange(LogLevel.Validate().Messages);

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
