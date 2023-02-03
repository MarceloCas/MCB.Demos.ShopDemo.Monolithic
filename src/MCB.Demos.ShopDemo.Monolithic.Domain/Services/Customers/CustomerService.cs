using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRegistered.Factories.Interfaces;
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
    // Constants
    public const string IMPORT_CUSTOMER_TRACE_NAME = $"{nameof(CustomerService)}.{nameof(ImportCustomerAsync)}";
    public const string DELETE_CUSTOMER_TRACE_NAME = $"{nameof(CustomerService)}.{nameof(DeleteCustomerAsync)}";

    // Messages
    public static readonly string CustomerEmailAlreadyRegisteredErrorCode = nameof(CustomerEmailAlreadyRegisteredErrorCode);
    public static readonly string CustomerEmailAlreadyRegisteredMessage = nameof(CustomerEmailAlreadyRegisteredMessage);
    public static readonly NotificationType CustomerEmailAlreadyRegisteredNotificationType = NotificationType.Error;

    public static readonly string CustomerNotFoundErrorCode = nameof(CustomerNotFoundErrorCode);
    public static readonly string CustomerNotFoundMessage = nameof(CustomerNotFoundMessage);
    public static readonly NotificationType CustomerNotFoundNotificationType = NotificationType.Error;

    // Fields
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerRegisteredDomainEventFactory _customerHasBeenRegisteredDomainEventFactory;
    private readonly ICustomerDeletedDomainEventFactory _customerDeletedDomainEventFactory;

    // Constructors
    public CustomerService(
        INotificationPublisher notificationPublisher,
        IDomainEventPublisher domainEventPublisher,
        ITraceManager traceManager,
        IAdapter adapter,
        ICustomerRepository customerRepository,
        ICustomerFactory customerFactory,
        ICustomerRegisteredDomainEventFactory customerHasBeenRegisteredDomainEventFactory,
        ICustomerDeletedDomainEventFactory customerDeletedDomainEventFactory
    ) : base(notificationPublisher, domainEventPublisher, traceManager, adapter, customerRepository)
    {
        _customerRepository = customerRepository;
        _customerFactory = customerFactory;
        _customerHasBeenRegisteredDomainEventFactory = customerHasBeenRegisteredDomainEventFactory;
        _customerDeletedDomainEventFactory = customerDeletedDomainEventFactory;
    }

    // Public Methods
    public Task<(bool Success, Customer? ImportedCustomer)> ImportCustomerAsync(ImportCustomerServiceInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: IMPORT_CUSTOMER_TRACE_NAME,
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

                    return default;
                }

                // Process
                var customer = input.CustomerFactory
                    .Create()!
                    .RegisterNewCustomer(input.Adapter.Adapt<ImportCustomerServiceInput, RegisterNewCustomerInput>(input.Input)!);

                // Validate domain entity after process
                if (!await ValidateDomainEntityAndSendNotificationsAsync(customer, cancellationToken))
                    return default;

                // Persist
                var persistenceResult = await input.CustomerRepository.ImportCustomerAsync(customer, cancellationToken);
                if (!persistenceResult.Success)
                    return default;

                // Send domain event
                await input.DomainEventPublisher.PublishDomainEventAsync(
                    input.CustomerHasBeenRegisteredDomainEventFactory.Create((customer, input.Input.ExecutionUser, input.Input.SourcePlatform, input.Input.CorrelationId))!,
                    cancellationToken
                );

                // Return
                return (Success: true, ImportedCustomer: customer);
            },
            cancellationToken
        )!;
    }
    public Task<(bool Success, Customer? DeletedCustomer)> DeleteCustomerAsync(DeleteCustomerServiceInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: DELETE_CUSTOMER_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, CustomerRepository: _customerRepository, Adapter, NotificationPublisher, CustomerFactory: _customerFactory, DomainEventPublisher, CustomerDeletedDomainEventFactory: _customerDeletedDomainEventFactory),
            handler: async (input, activity, cancellationToken) =>
            {
                // Validate input before process
                var customer = await input.CustomerRepository.GetByEmailAsync(input.Input.TenantId, input.Input.Email, cancellationToken);

                if (customer is null)
                {
                    await input.NotificationPublisher.PublishNotificationAsync(
                        new Notification(
                            CustomerNotFoundNotificationType,
                            CustomerNotFoundErrorCode,
                            CustomerNotFoundMessage
                        ),
                        cancellationToken
                    );

                    return default;
                }

                // Process
                var persistenceResult = await input.CustomerRepository.DeleteCustomerAsync(customer, cancellationToken);
                if (!persistenceResult)
                    return default;

                // Send domain event
                await input.DomainEventPublisher.PublishDomainEventAsync(
                    input.CustomerDeletedDomainEventFactory.Create((customer, input.Input.ExecutionUser, input.Input.SourcePlatform, input.Input.CorrelationId))!,
                    cancellationToken
                );

                // Return
                return (Success: persistenceResult, DeletedCustomer: customer);
            },
            cancellationToken
        )!;
    }
}