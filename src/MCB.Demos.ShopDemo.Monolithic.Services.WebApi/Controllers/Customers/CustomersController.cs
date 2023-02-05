﻿using Mapster;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.DeleteCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.DeleteCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Responses;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
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
    private readonly IValidateImportCustomerBatchUseCase _validateImportCustomerBatchUseCase;
    private readonly IDeleteCustomerUseCase _deleteCustomerUseCase;
    private readonly IGetCustomerByEmailQuery _getCustomerByEmailQuery;

    // Constructors
    public CustomersController(
        ILogger<CustomersController> logger,
        INotificationSubscriber notificationSubscriber,
        ITraceManager traceManager,
        IAdapter adapter,
        IMcbFeatureFlagManager featureFlagManager,
        IImportCustomerUseCase importCustomerUseCase,
        IImportCustomerBatchUseCase importCustomerBatchUseCase,
        IValidateImportCustomerBatchUseCase validateImportCustomerBatchUseCase,
        IDeleteCustomerUseCase deleteCustomerUseCase,
        IGetCustomerByEmailQuery getCustomerByEmailQuery
    ) : base(logger, notificationSubscriber, traceManager, adapter, featureFlagManager)
    {
        _importCustomerUseCase = importCustomerUseCase;
        _importCustomerBatchUseCase = importCustomerBatchUseCase;
        _validateImportCustomerBatchUseCase = validateImportCustomerBatchUseCase;
        _deleteCustomerUseCase = deleteCustomerUseCase;
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
            handler: async (input, activity, cancellationToken) => 
            {
                if (!await CheckFeatureFlagAsync(input.TenantId, executionUser: null, FeatureFlags.GET_CUSTOMER_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(FeatureFlags.GET_CUSTOMER_FEATURE_FLAG_KEY)!;

                return await RunQueryAsync(
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
                )!;
            },
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
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.IMPORT_CUSTOMER_BATCH_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(FeatureFlags.IMPORT_CUSTOMER_BATCH_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input!.ImportCustomerUseCase,
                    useCaseInput: input.Adapter.Adapt<ImportCustomerPayload, ImportCustomerUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => new ImportCustomerResponse(),
                    failResponseBaseFactory: (useCaseInput, useCaseOutput) => null,
                    successStatusCode: (int)System.Net.HttpStatusCode.Created,
                    failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
    }

    [HttpPost("validate-batch-import")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ValidateImportCustomerBatchResponse))]
    public Task<IActionResult> ValidateImportCustomerBatchAsync(
        [FromBody] ValidateImportCustomerBatchPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: nameof(Request.Path.Value) ?? nameof(ValidateImportCustomerBatchAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, ValidateImportCustomerBatchUseCase: _validateImportCustomerBatchUseCase, Adapter),
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.IMPORT_CUSTOMER_BATCH_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(FeatureFlags.IMPORT_CUSTOMER_BATCH_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input.ValidateImportCustomerBatchUseCase!,
                    useCaseInput: input.Adapter.Adapt<ValidateImportCustomerBatchPayload, ValidateImportCustomerBatchUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => Adapter.Adapt<ValidateImportCustomerBatchUseCaseResponse, ValidateImportCustomerBatchResponse>(useCaseOutput)!,
                    failResponseBaseFactory: (useCaseInput, useCaseOutput) => Adapter.Adapt<ValidateImportCustomerBatchUseCaseResponse, ValidateImportCustomerBatchResponse>(useCaseOutput)!,
                    successStatusCode: (int)System.Net.HttpStatusCode.OK,
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
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.IMPORT_CUSTOMER_BATCH_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(FeatureFlags.IMPORT_CUSTOMER_BATCH_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input.ImportCustomerBatchUseCase!,
                    useCaseInput: input.Adapter.Adapt<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => new ImportCustomerBatchResponse(),
                    failResponseBaseFactory: (useCaseInput, useCaseOutput) => null,
                    successStatusCode: (int)System.Net.HttpStatusCode.Created,
                    failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(DeleteCustomerResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseBase))]
    public Task<IActionResult> DeleteCustomerAsync(
        [FromBody] DeleteCustomerPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: nameof(Request.Path.Value) ?? nameof(DeleteCustomerAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, DeleteCustomerUseCase: _deleteCustomerUseCase, Adapter),
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.DELETE_CUSTOMER_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(FeatureFlags.DELETE_CUSTOMER_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input.DeleteCustomerUseCase!,
                    useCaseInput: input.Adapter.Adapt<DeleteCustomerPayload, DeleteCustomerUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => new DeleteCustomerResponse(),
                    failResponseBaseFactory: (useCaseInput, useCaseOutput) => null,
                    successStatusCode: (int)System.Net.HttpStatusCode.Created,
                    failStatusCode: (int)System.Net.HttpStatusCode.NotFound,
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
    }
}