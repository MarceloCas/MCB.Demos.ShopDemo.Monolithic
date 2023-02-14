using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Responses;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch;
public class ValidateImportProductBatchUseCase
    : UseCaseBase<ValidateImportProductBatchUseCaseInput, ValidateImportProductBatchUseCaseResponse>,
    IValidateImportProductBatchUseCase
{
    // Constants
    public const string PRODUCT_BATCH_IMPORT_FAIL_CODE = nameof(PRODUCT_BATCH_IMPORT_FAIL_CODE);
    public const string PRODUCT_BATCH_IMPORT_FAIL_MESSAGE = "Fail on import product batch|Index:{0}|Code:{1}";
    public const NotificationType PRODUCT_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE = NotificationType.Error;

    // Fields
    private readonly INotificationSubscriber _notificationSubscriber;
    private readonly IProductService _productService;

    // Constructors
    public ValidateImportProductBatchUseCase(
        IDomainEventSubscriber domainEventSubscriber,
        INotificationPublisher notificationPublisher,
        IEventsExchangeRabbitMqPublisher eventsExchangeRabbitMqPublisher,
        IExternalEventFactory externalEventFactory,
        ITraceManager traceManager,
        IAdapter adapter,
        IUnitOfWork unitOfWork,
        INotificationSubscriber notificationSubscriber,
        IProductService productService
    ) : base(domainEventSubscriber, notificationPublisher, eventsExchangeRabbitMqPublisher, externalEventFactory, traceManager, adapter, unitOfWork)
    {
        _notificationSubscriber = notificationSubscriber;
        _productService = productService;
    }

    // Public Methods
    protected override Task<(bool Success, ValidateImportProductBatchUseCaseResponse? Output)> ExecuteInternalAsync(ValidateImportProductBatchUseCaseInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: $"{nameof(ValidateImportProductBatchUseCase)}.{nameof(ExecuteInternalAsync)}",
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, UnitOfWork, ProductService: _productService, Adapter, NotificationSubscriber: _notificationSubscriber, NotificationPublisher),
            handler: (input, activity, cancellationToken) =>
            {
                return input.UnitOfWork.ExecuteAsync(
                    handler: async q =>
                    {
                        var validateImportProductBatchUseCaseResponseItemCollection = new List<ValidateImportProductBatchUseCaseResponseItem>();

                        for (int i = 0; i < q.Input.Input.Items.Length; i++)
                        {
                            var item = q.Input.Input.Items[i];

                            var processResult = await q.Input.ProductService.ValidateImportProductAsync(
                                input: q.Input.Adapter.Adapt<(ValidateImportProductBatchUseCaseInput, ValidateImportProductBatchUseCaseInputItem), ValidateImportProductServiceInput>((q.Input.Input!, item))!,
                                cancellationToken
                            );

                            validateImportProductBatchUseCaseResponseItemCollection.Add(
                                new ValidateImportProductBatchUseCaseResponseItem(
                                    index: i,
                                    code: item.Code,
                                    success: processResult.Success,
                                    notificationCollection: processResult.NotificationCollection
                                )
                            );
                        }

                        return (Success: !validateImportProductBatchUseCaseResponseItemCollection.Any(q => !q.Success), Output: new ValidateImportProductBatchUseCaseResponse(validateImportProductBatchUseCaseResponseItemCollection));
                    },
                    input: input,
                    openTransaction: false,
                    isBulkInsertOperation: false,
                    cancellationToken
                );
            },
            cancellationToken
        )!;
    }
}