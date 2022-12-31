using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;
using IUnitOfWork = MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces.IUnitOfWork;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch;

public class RegisterNewCustomerBatchUseCase
    : UseCaseBase<RegisterNewCustomerBatchUseCaseInput>,
    IRegisterNewCustomerBatchUseCase
{
    // Constants
    public const string CUSTOMER_BATCH_IMPORT_FAIL_CODE = nameof(CUSTOMER_BATCH_IMPORT_FAIL_CODE);
    public const string CUSTOMER_BATCH_IMPORT_FAIL_MESSAGE = "Fail on import customer batch|Index:{0}|Email:{1}";
    public const NotificationType CUSTOMER_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE = NotificationType.Error;

    // Fields
    private readonly INotificationSubscriber _notificationSubscriber;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICustomerService _customerService;

    // Constructors
    public RegisterNewCustomerBatchUseCase(
        INotificationPublisher notificationPublisher,
        IDomainEventSubscriber domainEventSubscriber,
        IExternalEventFactory externalEventFactory,
        IAdapter adapter,
        IUnitOfWork unitOfWork,
        INotificationSubscriber notificationSubscriber,
        IJsonSerializer jsonSerializer,
        ICustomerService customerService
    ) : base(notificationPublisher, domainEventSubscriber, externalEventFactory, adapter, unitOfWork)
    {
        _notificationSubscriber = notificationSubscriber;
        _jsonSerializer = jsonSerializer;
        _customerService = customerService;
    }

    // Public Methods
    protected override Task<bool> ExecuteInternalAsync(RegisterNewCustomerBatchUseCaseInput input, CancellationToken cancellationToken)
    {
        return UnitOfWork.ExecuteAsync(
            handler: async q =>
            {
                for (int i = 0; i < input.Items.Length; i++)
                {
                    var item = input.Items[i];

                    var processResult = await _customerService.RegisterNewCustomerAsync(
                        input: Adapter.Adapt<(RegisterNewCustomerBatchUseCaseInput, RegisterNewCustomerBatchUseCaseInputItem), RegisterNewCustomerServiceInput>((q.Input!, item))!,
                        cancellationToken
                    );

                    if (!processResult)
                    {
                        var notifications = _notificationSubscriber.NotificationCollection.ToArray();
                        _notificationSubscriber.ClearAllNotifications();

                        await NotificationPublisher.PublishNotificationAsync(
                            new Notification(
                                notificationType: CUSTOMER_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE,
                                code: CUSTOMER_BATCH_IMPORT_FAIL_CODE,
                                description: string.Format(
                                    CUSTOMER_BATCH_IMPORT_FAIL_MESSAGE,
                                    i,
                                    item.Email
                                ),
                                notificationCollection: notifications
                            ),
                            cancellationToken
                        );

                        return false;
                    }
                }

                return true;
            },
            input: input,
            openTransaction: true,
            cancellationToken
        );
    }
}
