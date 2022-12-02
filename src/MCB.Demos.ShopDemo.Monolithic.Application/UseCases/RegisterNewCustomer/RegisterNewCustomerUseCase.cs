using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer;

public class RegisterNewCustomerUseCase
    : UseCaseBase<RegisterNewCustomerUseCaseInput>,
    IRegisterNewCustomerUseCase
{
    // Fields
    private readonly ICustomerService _customerService;

    // Constructors
    public RegisterNewCustomerUseCase(
        IDomainEventSubscriber domainEventSubscriber,
        IExternalEventFactory externalEventFactory,
        IAdapter adapter,
        ICustomerService customerService
    ) : base(domainEventSubscriber, externalEventFactory, adapter)
    {
        _customerService = customerService;
    }

    // Public Methods
    protected override Task<bool> ExecuteInternalAsync(RegisterNewCustomerUseCaseInput input, CancellationToken cancellationToken)
    {
        return _customerService.RegisterNewCustomerAsync(
            input: Adapter.Adapt<RegisterNewCustomerUseCaseInput, RegisterNewCustomerServiceInput>(input)!,
            cancellationToken
        );
    }
}