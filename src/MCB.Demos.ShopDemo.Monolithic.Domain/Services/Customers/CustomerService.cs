using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers;

public class CustomerService
    : ServiceBase<Entities.Customers.Customer>,
    ICustomerService
{
    // Messages
    public static readonly string CustomerEmailAlreadyRegisteredErrorCode = nameof(CustomerEmailAlreadyRegisteredErrorCode);
    public static readonly string CustomerEmailAlreadyRegisteredMessage = nameof(CustomerEmailAlreadyRegisteredMessage);
    public static readonly NotificationType CustomerEmailAlreadyRegisteredNotificationType = NotificationType.Error;

    // Fields
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerHasBeenRegisteredDomainEventFactory _customerHasBeenRegisteredDomainEventFactory;

    // Constructors
    public CustomerService(
        INotificationPublisher notificationPublisher,
        IDomainEventPublisher domainEventPublisher,
        IAdapter adapter,
        ICustomerRepository customerRepository,
        ICustomerFactory customerFactory,
        ICustomerHasBeenRegisteredDomainEventFactory customerHasBeenRegisteredDomainEventFactory
    ) : base(notificationPublisher, domainEventPublisher, adapter, customerRepository)
    {
        _customerRepository = customerRepository;
        _customerFactory = customerFactory;
        _customerHasBeenRegisteredDomainEventFactory = customerHasBeenRegisteredDomainEventFactory;
    }

    // Public Methods
    public async Task<bool> RegisterNewCustomerAsync(RegisterNewCustomerServiceInput input, CancellationToken cancellationToken)
    {
        // Validate input before process
        if (await _customerRepository.GetByEmailAsync(input.TenantId, input.Email, cancellationToken) is not null)
        {
            await NotificationPublisher.PublishNotificationAsync(
                new Notification(
                    CustomerEmailAlreadyRegisteredNotificationType,
                    CustomerEmailAlreadyRegisteredErrorCode,
                    CustomerEmailAlreadyRegisteredMessage,
                    Enumerable.Empty<Notification>()
                ),
                cancellationToken
            );

            return false;
        }

        // Process
        var customer = _customerFactory
            .Create()!
            .RegisterNewCustomer(Adapter.Adapt<RegisterNewCustomerServiceInput, RegisterNewCustomerInput>(input)!);

        // Validate domain entity after process
        if (!await ValidateDomainEntityAndSendNotificationsAsync(customer, cancellationToken))
            return false;

        // Persist
        var persistenceResult = await _customerRepository.RegisterNewCustomerAsync(customer, cancellationToken);
        if (!persistenceResult.Success)
            return false;

        // Send domain event
        await DomainEventPublisher.PublishDomainEventAsync(
            _customerHasBeenRegisteredDomainEventFactory.Create((customer, input.ExecutionUser, input.SourcePlatform))!,
            cancellationToken
        );

        // Return
        return true;
    }
}