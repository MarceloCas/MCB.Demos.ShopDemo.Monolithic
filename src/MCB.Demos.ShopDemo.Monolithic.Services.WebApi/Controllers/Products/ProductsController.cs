using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Responses;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Payloads;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Responses;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class ProductsController
    : CustomControllerBase
{
    // Field
    private readonly IImportProductUseCase _importProductUseCase;
    private readonly IImportProductBatchUseCase _importProductBatchUseCase;
    private readonly IValidateImportProductBatchUseCase _validateImportProductBatchUseCase;
    private readonly IDeleteProductUseCase _deleteProductUseCase;
    private readonly IGetProductByCodeQuery _getProductByCodeQuery;

    // Constructors
    public ProductsController(
        ILogger<ProductsController> logger,
        INotificationSubscriber notificationSubscriber,
        ITraceManager traceManager,
        IAdapter adapter,
        IMcbFeatureFlagManager featureFlagManager,
        IImportProductUseCase importProductUseCase,
        IImportProductBatchUseCase importProductBatchUseCase,
        IValidateImportProductBatchUseCase validateImportProductBatchUseCase,
        IDeleteProductUseCase deleteProductUseCase,
        IGetProductByCodeQuery getProductByCodeQuery
    ) : base(logger, notificationSubscriber, traceManager, adapter, featureFlagManager)
    {
        _importProductUseCase = importProductUseCase;
        _importProductBatchUseCase = importProductBatchUseCase;
        _validateImportProductBatchUseCase = validateImportProductBatchUseCase;
        _deleteProductUseCase = deleteProductUseCase;
        _getProductByCodeQuery = getProductByCodeQuery;
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductReponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseBase))]
    public Task<IActionResult> GetProductCollectionAsync(
        [FromQuery] Guid correlationId,
        [FromQuery] Guid tenantId,
        [FromQuery] string executionUser,
        [FromQuery] string sourcePlatform,
        [FromQuery] string code,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: Request.Path.Value ?? nameof(ImportProductAsync),
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
                Code: code,
                GetProductByCodeQuery: _getProductByCodeQuery,
                Adapter
            ),
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.TenantId, executionUser: null, FeatureFlags.GET_PRODUCT_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(input.TenantId, FeatureFlags.GET_PRODUCT_FEATURE_FLAG_KEY)!;

                return await RunQueryAsync(
                    input,
                    handler: async (input, cancellationToken) =>
                    {
                        var getProductByQueryInput = new GetProductByCodeQueryInput(
                            input.CorrelationId,
                            input.TenantId,
                            input.ExecutionUser,
                            input.SourcePlatform,
                            input.Code
                        );

                        var queryResult = await input.GetProductByCodeQuery.ExecuteAsync(getProductByQueryInput, cancellationToken);

                        return queryResult is null ? null : input.Adapter.Adapt<ProductDto>(queryResult);
                    },
                    responseBaseFactory: () => new GetProductReponse(),
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
    }

    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportProductResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ImportProductResponse))]
    public Task<IActionResult> ImportProductAsync(
        [FromBody] ImportProductPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: Request.Path.Value ?? nameof(ImportProductAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, ImportProductUseCase: _importProductUseCase, Adapter),
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.IMPORT_PRODUCT_BATCH_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(input.Payload.TenantId, FeatureFlags.IMPORT_PRODUCT_BATCH_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input!.ImportProductUseCase,
                    useCaseInput: input.Adapter.Adapt<ImportProductPayload, ImportProductUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => new ImportProductResponse(),
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ValidateImportProductBatchResponse))]
    public Task<IActionResult> ValidateImportProductBatchAsync(
        [FromBody] ValidateImportProductBatchPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: nameof(Request.Path.Value) ?? nameof(ValidateImportProductBatchAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, ValidateImportProductBatchUseCase: _validateImportProductBatchUseCase, Adapter),
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.IMPORT_PRODUCT_BATCH_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(input.Payload.TenantId, FeatureFlags.IMPORT_PRODUCT_BATCH_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input.ValidateImportProductBatchUseCase!,
                    useCaseInput: input.Adapter.Adapt<ValidateImportProductBatchPayload, ValidateImportProductBatchUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => Adapter.Adapt<ValidateImportProductBatchUseCaseResponse, ValidateImportProductBatchResponse>(useCaseOutput)!,
                    failResponseBaseFactory: (useCaseInput, useCaseOutput) => Adapter.Adapt<ValidateImportProductBatchUseCaseResponse, ValidateImportProductBatchResponse>(useCaseOutput)!,
                    successStatusCode: (int)System.Net.HttpStatusCode.OK,
                    failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
    }

    [HttpPost("batch-import")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportProductBatchResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ImportProductBatchResponse))]
    public Task<IActionResult> ImportProductBatchAsync(
        [FromBody] ImportProductBatchPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: nameof(Request.Path.Value) ?? nameof(ImportProductBatchAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, ImportProductBatchUseCase: _importProductBatchUseCase, Adapter),
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.IMPORT_PRODUCT_BATCH_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(input.Payload.TenantId, FeatureFlags.IMPORT_PRODUCT_BATCH_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input.ImportProductBatchUseCase!,
                    useCaseInput: input.Adapter.Adapt<ImportProductBatchPayload, ImportProductBatchUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => new ImportProductBatchResponse(),
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
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(DeleteProductResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseBase))]
    public Task<IActionResult> DeleteProductAsync(
        [FromBody] DeleteProductPayload payload,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartActivityAsync(
            name: nameof(Request.Path.Value) ?? nameof(DeleteProductAsync),
            kind: System.Diagnostics.ActivityKind.Server,
            correlationId: payload.CorrelationId,
            tenantId: payload.TenantId,
            executionUser: payload.ExecutionUser,
            sourcePlatform: payload.SourcePlatform,
            input: (Payload: payload, DeleteProductUseCase: _deleteProductUseCase, Adapter),
            handler: async (input, activity, cancellationToken) =>
            {
                if (!await CheckFeatureFlagAsync(input.Payload.TenantId, executionUser: null, FeatureFlags.DELETE_PRODUCT_FEATURE_FLAG_KEY, cancellationToken))
                    return CreateNotAllowedResult(input.Payload.TenantId, FeatureFlags.DELETE_PRODUCT_FEATURE_FLAG_KEY)!;

                return await RunUseCaseAsync(
                    useCase: input.DeleteProductUseCase!,
                    useCaseInput: input.Adapter.Adapt<DeleteProductPayload, DeleteProductUseCaseInput>(input.Payload)!,
                    successResponseBaseFactory: (useCaseInput, useCaseOutput) => new DeleteProductResponse(),
                    failResponseBaseFactory: (useCaseInput, useCaseOutput) => new DeleteProductResponse(),
                    successStatusCode: (int)System.Net.HttpStatusCode.Created,
                    failStatusCode: (int)System.Net.HttpStatusCode.NotFound,
                    cancellationToken
                )!;
            },
            cancellationToken
        )!;
    }
}