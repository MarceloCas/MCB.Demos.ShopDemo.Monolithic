using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin.Payloads;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin.Responses;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin;

[Route("admin/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class ResiliencePoliciesController
    : CustomControllerBase
{
    // Fields
    private static IResiliencePolicy[] _resiliencePolicyCollection = null!;

    // Constructors
    public ResiliencePoliciesController(
        ILogger<ResiliencePoliciesController> logger,
        INotificationSubscriber notificationSubscriber, 
        ITraceManager traceManager, 
        IAdapter adapter
    ) : base(logger, notificationSubscriber, traceManager, adapter)
    {

    }

    // Public Methods
    public static void SetResiliencePolicyCollection(IEnumerable<IResiliencePolicy> resiliencePolicyCollection)
    {
        _resiliencePolicyCollection = resiliencePolicyCollection.ToArray();
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResiliencePolicyCollectionResponse))]
    public GetResiliencePolicyCollectionResponse GetResiliencePolicyCollection()
    {
        return new GetResiliencePolicyCollectionResponse
        {
            ResiliencePolicyCollection = _resiliencePolicyCollection.Select(q => 
                new ResiliencePolicyDto { 
                    Code = q.GetType().FullName!, 
                    Name = q.Name,
                    CircuitState = q.CircuitState.ToString(), 
                    CurrentCircuitBreakerOpenCount = q.CurrentCircuitBreakerOpenCount 
                }
            )
        };
    }

    [HttpPost("open-circuit-breaker")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult OpenCircuitBreaker([FromBody] OpenCircuitBreakerPayload payload)
    {
        if (payload?.Code == null)
            return BadRequest("code cannot be null");

        var resiliencePolicy = _resiliencePolicyCollection.FirstOrDefault(q => q.GetType().FullName == payload.Code);

        if (resiliencePolicy == null)
            return NotFound("resiliencePolicy not found");

        resiliencePolicy.OpenCircuitBreakerManually();

        return Ok();
    }
    [HttpPost("close-circuit-breaker")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult CloseCircuitBreaker([FromBody] CloseCircuitBreakerPayload payload)
    {
        if (payload?.Code == null)
            return BadRequest("code cannot be null");

        var resiliencePolicy = _resiliencePolicyCollection.FirstOrDefault(q => q.GetType().FullName == payload.Code);

        if (resiliencePolicy == null)
            return NotFound("resiliencePolicy not found");

        resiliencePolicy.CloseCircuitBreakerManually();

        return Ok();
    }
}
