using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;

public class StartupCheck
    : IHealthCheck
{
    // Properties
    public static bool StartupCompleted { get; private set; }

    // Public Methods
    public static void CompleteStartup()
    {
        StartupCompleted = true;
    }
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            StartupCompleted
                ? HealthCheckResult.Healthy()
                : new HealthCheckResult(status: context.Registration.FailureStatus)
        );
    }
}
