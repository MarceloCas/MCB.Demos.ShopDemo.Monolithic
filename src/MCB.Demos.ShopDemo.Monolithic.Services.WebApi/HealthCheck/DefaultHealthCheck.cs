using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;

public class DefaultHealthCheck
    : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        // ...

        if (!isHealthy)
            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "An unhealthy result."));

        return Task.FromResult(HealthCheckResult.Healthy("A healthy result."));

    }
}