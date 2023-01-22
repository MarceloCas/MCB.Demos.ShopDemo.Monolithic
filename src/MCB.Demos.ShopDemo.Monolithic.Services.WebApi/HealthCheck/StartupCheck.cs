using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;

public class StartupCheck
    : IHealthCheck
{
    // Constants
    public const string START_UP_NO_COMPLETED = nameof(START_UP_NO_COMPLETED);

    // Fields
    private readonly IStartupService _startupService;

    // Constructors
    public StartupCheck(IStartupService startupService)
    {
        _startupService = startupService;
    }

    // Public Methods
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            _startupService.HasStarted
                ? HealthCheckResult.Healthy()
                : new HealthCheckResult(
                    status: context.Registration.FailureStatus, 
                    data: new Dictionary<string, object>() { { START_UP_NO_COMPLETED, ServiceStatus.Unhealthy } }
                )
        );
    }
}
