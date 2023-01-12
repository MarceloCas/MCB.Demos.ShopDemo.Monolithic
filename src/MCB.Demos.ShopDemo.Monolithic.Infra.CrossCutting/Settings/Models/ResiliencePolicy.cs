namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

public class ResiliencePolicy
{
    public string Name { get; set; } = null!;
    public int RetryMaxAttemptCount { get; set; }
    public int RetryAttemptWaitingTimeMilliseconds { get; set; }
    public int CircuitBreakerWaitingTimeSeconds { get; set; }
}
