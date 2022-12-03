using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers;

[Route("api/[controller]")]
[ApiController]
[ApiVersion("1")]
public class CustomersController
    : CustomControllerBase
{
    // Field
    private readonly IRegisterNewCustomerUseCase _registerNewCustomerUseCase;

    // Constructors
    public CustomersController(
        INotificationSubscriber notificationSubscriber,
        IAdapter adapter,
        IRegisterNewCustomerUseCase registerNewCustomerUseCase
    ) : base(notificationSubscriber, adapter)
    {
        _registerNewCustomerUseCase = registerNewCustomerUseCase;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterNewCustomerResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(RegisterNewCustomerResponse))]
    public async Task<IActionResult> RegisterNewCustomerAsync(
        [FromBody] RegisterNewCustomerPayload payload,
        CancellationToken cancellationToken
    )
    {
        return await RunUseCaseAsync(
            useCase: _registerNewCustomerUseCase!,
            useCaseInput: Adapter.Adapt<RegisterNewCustomerPayload, RegisterNewCustomerUseCaseInput>(payload)!,
            responseBaseFactory: (useCaseInput) => new RegisterNewCustomerResponse(),
            successStatusCode: 201,
            failStatusCode: 422,
            cancellationToken
        );
    }
}