using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch;
public class ImportProductBatchUseCase
    : UseCaseBase<ImportProductBatchUseCaseInput, int>,
    IImportProductBatchUseCase
{
    // Constants
    public const string PRODUCT_BATCH_IMPORT_FAIL_CODE = nameof(PRODUCT_BATCH_IMPORT_FAIL_CODE);
    public const string PRODUCT_BATCH_IMPORT_FAIL_MESSAGE = "Fail on import product batch|Index:{0}|Code:{1}";
    public const NotificationType PRODUCT_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE = NotificationType.Error;

    // Fields
    private readonly INotificationSubscriber _notificationSubscriber;
    private readonly IProductService _productService;

    // Constructors
    public ImportProductBatchUseCase(
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
    protected override Task<(bool Success, int Output)> ExecuteInternalAsync(ImportProductBatchUseCaseInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: $"{nameof(ImportProductBatchUseCase)}.{nameof(ExecuteInternalAsync)}",
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
                        for (int i = 0; i < q.Input.Input.Items.Length; i++)
                        {
                            var item = q.Input.Input.Items[i];

                            var processResult = await q.Input.ProductService.ImportProductAsync(
                                input: q.Input.Adapter.Adapt<(ImportProductBatchUseCaseInput, ImportProductBatchUseCaseInputItem), ImportProductServiceInput>((q.Input.Input!, item))!,
                                cancellationToken
                            );

                            if (!processResult.Success)
                            {
                                var notifications = q.Input.NotificationSubscriber.NotificationCollection.ToArray();
                                q.Input.NotificationSubscriber.ClearAllNotifications();

                                await q.Input.NotificationPublisher.PublishNotificationAsync(
                                    new Notification(
                                        notificationType: PRODUCT_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE,
                                        code: PRODUCT_BATCH_IMPORT_FAIL_CODE,
                                        description: string.Format(
                                            PRODUCT_BATCH_IMPORT_FAIL_MESSAGE,
                                            i,
                                            item.Code
                                        ),
                                        notificationCollection: notifications
                                    ),
                                    cancellationToken
                                );

                                return default;
                            }
                        }

                        return (Success: true, Output: input.Input.Items.Length);
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
