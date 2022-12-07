using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Interfaces;
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
    private readonly IRegisterNewCustomerUseCase _registerNewCustomerUseCase;
    private readonly IRegisterNewCustomerBatchUseCase _registerNewCustomerBatchUseCase;

    // Constructors
    public CustomersController(
        INotificationSubscriber notificationSubscriber,
        IAdapter adapter,
        IRegisterNewCustomerUseCase registerNewCustomerUseCase,
        IRegisterNewCustomerBatchUseCase registerNewCustomerBatchUseCase
    ) : base(notificationSubscriber, adapter)
    {
        _registerNewCustomerUseCase = registerNewCustomerUseCase;
        _registerNewCustomerBatchUseCase = registerNewCustomerBatchUseCase;
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
            useCase: _registerNewCustomerUseCase!,
            useCaseInput: Adapter.Adapt<ImportCustomerPayload, RegisterNewCustomerUseCaseInput>(payload)!,
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
            useCase: _registerNewCustomerBatchUseCase!,
            useCaseInput: Adapter.Adapt<ImportCustomerBatchPayload, RegisterNewCustomerBatchUseCaseInput>(payload)!,
            responseBaseFactory: (useCaseInput) => new ImportCustomerBatchResponse(),
            successStatusCode: (int)System.Net.HttpStatusCode.Created,
            failStatusCode: (int)System.Net.HttpStatusCode.UnprocessableEntity,
            cancellationToken
        );
    }
}