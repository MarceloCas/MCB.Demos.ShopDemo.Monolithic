using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Responses;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch;
public class ValidateImportCustomerBatchUseCase
    : UseCaseBase<ValidateImportCustomerBatchUseCaseInput, ValidateImportCustomerBatchUseCaseResponse>,
    IValidateImportCustomerBatchUseCase
{
    // Constants
    public const string CUSTOMER_BATCH_IMPORT_FAIL_CODE = nameof(CUSTOMER_BATCH_IMPORT_FAIL_CODE);
    public const string CUSTOMER_BATCH_IMPORT_FAIL_MESSAGE = "Fail on import customer batch|Index:{0}|Email:{1}";
    public const NotificationType CUSTOMER_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE = NotificationType.Error;

    // Fields
    private readonly INotificationSubscriber _notificationSubscriber;
    private readonly ICustomerService _customerService;

    // Constructors
    public ValidateImportCustomerBatchUseCase(
        IDomainEventSubscriber domainEventSubscriber,
        INotificationPublisher notificationPublisher,
        IEventsExchangeRabbitMqPublisher eventsExchangeRabbitMqPublisher,
        IExternalEventFactory externalEventFactory,
        ITraceManager traceManager,
        IAdapter adapter,
        IUnitOfWork unitOfWork,
        INotificationSubscriber notificationSubscriber,
        ICustomerService customerService
    ) : base(domainEventSubscriber, notificationPublisher, eventsExchangeRabbitMqPublisher, externalEventFactory, traceManager, adapter, unitOfWork)
    {
        _notificationSubscriber = notificationSubscriber;
        _customerService = customerService;
    }

    // Public Methods
    protected override Task<(bool Success, ValidateImportCustomerBatchUseCaseResponse? Output)> ExecuteInternalAsync(ValidateImportCustomerBatchUseCaseInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: $"{nameof(ValidateImportCustomerBatchUseCase)}.{nameof(ExecuteInternalAsync)}",
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, UnitOfWork, CustomerService: _customerService, Adapter, NotificationSubscriber: _notificationSubscriber, NotificationPublisher),
            handler: (input, activity, cancellationToken) =>
            {
                return input.UnitOfWork.ExecuteAsync(
                    handler: async q =>
                    {
                        var validateImportCustomerBatchUseCaseResponseItemCollection = new List<ValidateImportCustomerBatchUseCaseResponseItem>();

                        for (int i = 0; i < q.Input.Input.Items.Length; i++)
                        {
                            var item = q.Input.Input.Items[i];

                            var processResult = await q.Input.CustomerService.ValidateImportCustomerAsync(
                                input: q.Input.Adapter.Adapt<(ValidateImportCustomerBatchUseCaseInput, ValidateImportCustomerBatchUseCaseInputItem), ValidateImportCustomerServiceInput>((q.Input.Input!, item))!,
                                cancellationToken
                            );

                            validateImportCustomerBatchUseCaseResponseItemCollection.Add(
                                new ValidateImportCustomerBatchUseCaseResponseItem(
                                    index: i,
                                    email: item.Email,
                                    success: processResult.Success,
                                    notificationCollection: processResult.NotificationCollection
                                )
                            );
                        }

                        return (Success: !validateImportCustomerBatchUseCaseResponseItemCollection.Any(q => !q.Success), Output: new ValidateImportCustomerBatchUseCaseResponse(validateImportCustomerBatchUseCaseResponseItemCollection));
                    },
                    input: input,
                    openTransaction: false,
                    isBulkInsertOperation: true,
                    cancellationToken
                );
            },
            cancellationToken
        )!;
    }
}