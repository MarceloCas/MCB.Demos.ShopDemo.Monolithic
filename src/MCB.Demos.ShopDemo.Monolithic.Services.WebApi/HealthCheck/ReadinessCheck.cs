using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;

public class ReadinessCheck
    : IHealthCheck
{
    // Public Methods
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            StartupCheck.StartupCompleted
             ? HealthCheckResult.Healthy()
             : new HealthCheckResult(status: context.Registration.FailureStatus)
        );
    }
}