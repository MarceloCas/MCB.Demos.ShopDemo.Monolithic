namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

public class ResiliencePolicy
{
    // Properties
    public string Name { get; set; } = null!;
    public int RetryMaxAttemptCount { get; set; }
    public int RetryAttemptWaitingTimeMilliseconds { get; set; }
    public int CircuitBreakerWaitingTimeSeconds { get; set; }

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        if (string.IsNullOrEmpty(Name))
            messageCollection.Add($"{typeof(ResiliencePolicy).FullName}.{nameof(Name)} cannot be null");

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
