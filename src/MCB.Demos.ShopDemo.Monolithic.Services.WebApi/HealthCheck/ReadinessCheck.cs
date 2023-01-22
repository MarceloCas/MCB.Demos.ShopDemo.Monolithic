using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;

public class ReadinessCheck
    : IHealthCheck
{
    // Constants
    public const string NOT_READY = nameof(NOT_READY);

    // Fields
    private IStartupService _startupService;

    // Constructors
    public ReadinessCheck(IStartupService startupService)
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
                    data: new Dictionary<string, object>() { { NOT_READY, ServiceStatus.Unhealthy } }
                )
        );
    }
}