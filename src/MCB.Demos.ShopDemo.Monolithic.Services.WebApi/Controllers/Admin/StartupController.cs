using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin;

[Route("admin/v{version:apiVersion}/startup")]
[ApiController]
[ApiVersion("1")]
public class StartupController
    : CustomControllerBase
{
    // Fields
    private readonly IStartupService _startupService;

    // Constructors
    public StartupController(
        ILogger<ResiliencePoliciesController> logger,
        INotificationSubscriber notificationSubscriber,
        ITraceManager traceManager,
        IAdapter adapter,
        IStartupService startupService,
        IMcbFeatureFlagManager featureFlagManager
    ) : base(logger, notificationSubscriber, traceManager, adapter, featureFlagManager)
    {
        _startupService = startupService;
    }

    // Public Methods
    [HttpPost("try-startup-application")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> TryStartupApplicationAsync(CancellationToken cancellationToken)
    {
        var tryStartupApplicationResult = await _startupService.TryStartupApplicationAsync(cancellationToken);

        return tryStartupApplicationResult.Success ? Ok() : (IActionResult)StatusCode(503);
    }

}
