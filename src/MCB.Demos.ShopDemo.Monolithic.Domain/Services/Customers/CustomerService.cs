using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
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
        ITraceManager traceManager,
        IAdapter adapter,
        ICustomerRepository customerRepository,
        ICustomerFactory customerFactory,
        ICustomerHasBeenRegisteredDomainEventFactory customerHasBeenRegisteredDomainEventFactory
    ) : base(notificationPublisher, domainEventPublisher, traceManager, adapter, customerRepository)
    {
        _customerRepository = customerRepository;
        _customerFactory = customerFactory;
        _customerHasBeenRegisteredDomainEventFactory = customerHasBeenRegisteredDomainEventFactory;
    }

    // Public Methods
    public Task<bool> ImportCustomerAsync(ImportCustomerServiceInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: $"{nameof(CustomerService)}.{nameof(ImportCustomerAsync)}",
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, CustomerRepository: _customerRepository, Adapter, NotificationPublisher, CustomerFactory: _customerFactory, DomainEventPublisher, CustomerHasBeenRegisteredDomainEventFactory: _customerHasBeenRegisteredDomainEventFactory),
            handler: async (input, activity, cancellationToken) =>
            {
                // Validate input before process
                if (await input.CustomerRepository.GetByEmailAsync(input.Input.TenantId, input.Input.Email, cancellationToken) is not null)
                {
                    await input.NotificationPublisher.PublishNotificationAsync(
                        new Notification(
                            CustomerEmailAlreadyRegisteredNotificationType,
                            CustomerEmailAlreadyRegisteredErrorCode,
                            CustomerEmailAlreadyRegisteredMessage
                        ),
                        cancellationToken
                    );

                    return false;
                }

                // Process
                var customer = input.CustomerFactory
                    .Create()!
                    .RegisterNewCustomer(input.Adapter.Adapt<ImportCustomerServiceInput, RegisterNewCustomerInput>(input.Input)!);

                // Validate domain entity after process
                if (!await ValidateDomainEntityAndSendNotificationsAsync(customer, cancellationToken))
                    return false;

                // Persist
                var persistenceResult = await input.CustomerRepository.ImportCustomerAsync(customer, cancellationToken);
                if (!persistenceResult.Success)
                    return false;

                // Send domain event
                await input.DomainEventPublisher.PublishDomainEventAsync(
                    input.CustomerHasBeenRegisteredDomainEventFactory.Create((customer, input.Input.ExecutionUser, input.Input.SourcePlatform))!,
                    cancellationToken
                );

                // Return
                return true;
            },
            cancellationToken
        )!;
    }
}