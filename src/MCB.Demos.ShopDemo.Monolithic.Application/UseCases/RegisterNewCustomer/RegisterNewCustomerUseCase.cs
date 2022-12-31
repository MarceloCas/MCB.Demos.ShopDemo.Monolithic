using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer;

public class RegisterNewCustomerUseCase
    : UseCaseBase<RegisterNewCustomerUseCaseInput>,
    IRegisterNewCustomerUseCase
{
    // Fields
    private readonly ICustomerService _customerService;

    // Constructors
    public RegisterNewCustomerUseCase(
        INotificationPublisher notificationPublisher,
        IDomainEventSubscriber domainEventSubscriber,
        IExternalEventFactory externalEventFactory,
        IAdapter adapter,
        IUnitOfWork unitOfWork,
        ICustomerService customerService
    ) : base(notificationPublisher, domainEventSubscriber, externalEventFactory, adapter, unitOfWork)
    {
        _customerService = customerService;
    }

    // Public Methods
    protected override Task<bool> ExecuteInternalAsync(RegisterNewCustomerUseCaseInput input, CancellationToken cancellationToken)
    {
        return UnitOfWork.ExecuteAsync(
            handler: q =>
            {
                return _customerService.RegisterNewCustomerAsync(
                    input: Adapter.Adapt<RegisterNewCustomerUseCaseInput, RegisterNewCustomerServiceInput>(q.Input)!,
                    cancellationToken
                );
            },
            input: input,
            openTransaction: false,
            cancellationToken
        );
    }
}