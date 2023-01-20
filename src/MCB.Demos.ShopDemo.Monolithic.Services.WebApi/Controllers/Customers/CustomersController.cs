using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Models;
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
    private readonly IGetCustomerByEmailQuery _getCustomerByEmailQuery;

    // Constructors
    public CustomersController(
        ILogger<CustomersController> logger,
        INotificationSubscriber notificationSubscriber,
        ITraceManager traceManager,
        IAdapter adapter,
        IImportCustomerUseCase importCustomerUseCase,
        IImportCustomerBatchUseCase importCustomerBatchUseCase,
        IGetCustomerByEmailQuery getCustomerByEmailQuery
    ) : base(logger, notificationSubscriber, traceManager, adapter)
    {
        _importCustomerUseCase = importCustomerUseCase;
        _importCustomerBatchUseCase = importCustomerBatchUseCase;
        _getCustomerByEmailQuery = getCustomerByEmailQuery;
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCustomerReponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseBase))]
    public Task<IActionResult> GetCustomerCollectionAsync(
        [FromQuery]Guid correlationId,
        [FromQuery]Guid tenantId,
        [FromQuery]string executionUser,
        [FromQuery]string sourcePlatform,
        [FromQuery] string email,
        CancellationToken cancellationToken    
    )
    {
        return TraceManager.StartActivityAsync(
            name: Request.Path.Value ?? nameof(ImportCustomerAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: correlationId,
            tenantId: tenantId,
            executionUser: executionUser,
            sourcePlatform: sourcePlatform,
            input: (
                CorrelationId: correlationId, 
                TenantId: tenantId,
                ExecutionUser: executionUser,
                SourcePlatform: sourcePlatform,
                Email: email,
                GetCustomerByEmailQuery: _getCustomerByEmailQuery,
                Adapter
            ),
            handler: (input, activity, cancellationToken) => 
                RunQueryAsync(
                    input,
                    handler: async (input, cancellationToken) =>
                    {
                        var getCustomerByQueryInput = new GetCustomerByEmailQueryInput(
                            input.CorrelationId,
                            input.TenantId,
                            input.ExecutionUser,
                            input.SourcePlatform,
                            input.Email
                        );

                        var queryResult = await input.GetCustomerByEmailQuery.ExecuteAsync(getCustomerByQueryInput, cancellationToken);

                        return queryResult is null ? null : input.Adapter.Adapt<CustomerDto>(queryResult);
                    },
                    responseBaseFactory: () => new GetCustomerReponse(),
                    cancellationToken
                )!
            ,
            cancellationToken
        )!;
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
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, ImportCustomerUseCase: _importCustomerUseCase, Adapter),
            handler: (input, activity, cancellationToken) =>
            {
                return RunUseCaseAsync(
                    useCase: input!.ImportCustomerUseCase,
                    useCaseInput: input.Adapter.Adapt<ImportCustomerPayload, ImportCustomerUseCaseInput>(input.Payload)!,
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
            name: nameof(Request.Path.Value) ?? nameof(ImportCustomerBatchAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, ImportCustomerBatchUseCase: _importCustomerBatchUseCase, Adapter),
            handler: (input, activity, cancellationToken) =>
            {
                return RunUseCaseAsync(
                    useCase: input.ImportCustomerBatchUseCase!,
                    useCaseInput: input.Adapter.Adapt<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>(input.Payload)!,
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