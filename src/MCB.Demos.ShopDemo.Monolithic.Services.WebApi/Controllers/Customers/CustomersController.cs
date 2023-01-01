using Mapster;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class CustomersController
    : CustomControllerBase
{
    // Field
    private readonly IImportCustomerUseCase _importCustomerUseCase;
    private readonly IImportCustomerBatchUseCase _importCustomerBatchUseCase;

    // Constructors
    public CustomersController(
        INotificationSubscriber notificationSubscriber,
        ITraceManager traceManager,
        IAdapter adapter,
        IImportCustomerUseCase importCustomerUseCase,
        IImportCustomerBatchUseCase importCustomerBatchUseCase
    ) : base(notificationSubscriber, traceManager, adapter)
    {
        _importCustomerUseCase = importCustomerUseCase;
        _importCustomerBatchUseCase = importCustomerBatchUseCase;
    }

    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportCustomerResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ImportCustomerResponse))]
    public Task<IActionResult> ImportCustomerAsync(
        [FromBody] ImportCustomerPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: Request.Path.Value ?? nameof(ImportCustomerAsync),
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (payload, _importCustomerUseCase, Adapter),
            handler: (input, activity, cancellationToken) =>
            {
                return RunUseCaseAsync(
                    useCase: input._importCustomerUseCase!,
                    useCaseInput: input.Adapter.Adapt<ImportCustomerPayload, ImportCustomerUseCaseInput>(input.payload)!,
                    responseBaseFactory: (useCaseInput) => new ImportCustomerResponse(),
                    successStatusCode: (int)System.Net.HttpStatusCode.Created,
                    failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
        
    }


    [HttpPost("batch-import")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportCustomerBatchResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ImportCustomerBatchResponse))]
    public Task<IActionResult> ImportCustomerBatchAsync(
        [FromBody] ImportCustomerBatchPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: nameof(Request.Path.Value),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (payload, _importCustomerBatchUseCase, Adapter),
            handler: (input, activity, cancellationToken) =>
            {
                return RunUseCaseAsync(
                    useCase: input._importCustomerBatchUseCase!,
                    useCaseInput: input.Adapter.Adapt<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>(input.payload)!,
                    responseBaseFactory: (useCaseInput) => new ImportCustomerBatchResponse(),
                    successStatusCode: (int)System.Net.HttpStatusCode.Created,
                    failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
        
    }
}