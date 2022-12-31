using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
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
        IAdapter adapter,
        IImportCustomerUseCase importCustomerUseCase,
        IImportCustomerBatchUseCase importCustomerBatchUseCase
    ) : base(notificationSubscriber, adapter)
    {
        _importCustomerUseCase = importCustomerUseCase;
        _importCustomerBatchUseCase = importCustomerBatchUseCase;
    }

    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportCustomerResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ImportCustomerResponse))]
    public async Task<IActionResult> ImportCustomerAsync(
        [FromBody] ImportCustomerPayload payload,
        CancellationToken cancellationToken
    )
    {
        return await RunUseCaseAsync(
            useCase: _importCustomerUseCase!,
            useCaseInput: Adapter.Adapt<ImportCustomerPayload, ImportCustomerUseCaseInput>(payload)!,
            responseBaseFactory: (useCaseInput) => new ImportCustomerResponse(),
            successStatusCode: (int) System.Net.HttpStatusCode.Created,
            failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
            cancellationToken
        );
    }


    [HttpPost("batch-import")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportCustomerBatchResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ImportCustomerBatchResponse))]
    public async Task<IActionResult> ImportCustomerBatchAsync(
        [FromBody] ImportCustomerBatchPayload payload,
        CancellationToken cancellationToken
    )
    {
        return await RunUseCaseAsync(
            useCase: _importCustomerBatchUseCase!,
            useCaseInput: Adapter.Adapt<ImportCustomerBatchPayload, ImportCustomerBatchUseCaseInput>(payload)!,
            responseBaseFactory: (useCaseInput) => new ImportCustomerBatchResponse(),
            successStatusCode: (int)System.Net.HttpStatusCode.Created,
            failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
            cancellationToken
        );
    }
}