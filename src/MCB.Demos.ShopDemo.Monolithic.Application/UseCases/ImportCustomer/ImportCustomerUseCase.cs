using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer;

public class ImportCustomerUseCase
    : UseCaseBase<ImportCustomerUseCaseInput>,
    IImportCustomerUseCase
{
    // Constants
    public const string IMPORT_CUSTOMER_USE_CASE_TRACE_NAME = $"{nameof(ImportCustomerUseCase)}.{nameof(ExecuteInternalAsync)}";

    // Fields
    private readonly ICustomerService _customerService;

    // Constructors
    public ImportCustomerUseCase(
        IDomainEventSubscriber domainEventSubscriber,
        INotificationPublisher notificationPublisher,
        IEventsExchangeRabbitMqPublisher eventsExchangeRabbitMqPublisher,
        IExternalEventFactory externalEventFactory,
        ITraceManager traceManager,
        IAdapter adapter,
        IUnitOfWork unitOfWork,
        ICustomerService customerService
    ) : base(domainEventSubscriber, notificationPublisher, eventsExchangeRabbitMqPublisher, externalEventFactory, traceManager, adapter, unitOfWork)
    {
        _customerService = customerService;
    }

    // Public Methods
    protected override Task<bool> ExecuteInternalAsync(ImportCustomerUseCaseInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: IMPORT_CUSTOMER_USE_CASE_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, UnitOfWork, CustomerService: _customerService, Adapter),
            handler: (input, activity, cancellationToken) =>
            {
                return input.UnitOfWork.ExecuteAsync(
                    handler: q =>
                    {
                        return q.Input.CustomerService.ImportCustomerAsync(
                            input: q.Input.Adapter.Adapt<ImportCustomerUseCaseInput, ImportCustomerServiceInput>(q.Input.Input)!,
                            cancellationToken
                        );
                    },
                    input: input,
                    openTransaction: false,
                    cancellationToken
                );
            },
            cancellationToken
        )!;
    }
}