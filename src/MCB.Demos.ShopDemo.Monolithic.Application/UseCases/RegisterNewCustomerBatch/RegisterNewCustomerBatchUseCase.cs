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
    public const string CUSTOMER_BATCH_IMPORT_FAIL_MESSAGE = "Fail on import customer batch|Index:{0}|Customer:{1}";
    public const NotificationType CUSTOMER_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE = NotificationType.Error;

    // Fields
    private readonly INotificationPublisher _notificationPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICustomerService _customerService;

    // Constructors
    public RegisterNewCustomerBatchUseCase(
        IDomainEventSubscriber domainEventSubscriber,
        IExternalEventFactory externalEventFactory,
        IAdapter adapter,
        INotificationPublisher notificationPublisher,
        IUnitOfWork unitOfWork,
        IJsonSerializer jsonSerializer,
        ICustomerService customerService
    ) : base(domainEventSubscriber, externalEventFactory, adapter)
    {
        _notificationPublisher = notificationPublisher;
        _unitOfWork = unitOfWork;
        _jsonSerializer = jsonSerializer;
        _customerService = customerService;
    }

    // Public Methods
    protected override Task<bool> ExecuteInternalAsync(RegisterNewCustomerBatchUseCaseInput input, CancellationToken cancellationToken)
    {
        return _unitOfWork.ExecuteAsync(
            handler: async q =>
            {
                for (int i = 0; i < input.Items.Length; i++)
                {
                    var item = input.Items[i];

                    var processResult = await _customerService.RegisterNewCustomerAsync(
                        input: Adapter.Adapt<RegisterNewCustomerBatchUseCaseInputItem, RegisterNewCustomerServiceInput>(item)!,
                        cancellationToken
                    );

                    if (!processResult)
                    {
                        await _notificationPublisher.PublishNotificationAsync(
                            new Notification(
                                notificationType: CUSTOMER_BATCH_IMPORT_FAIL_NOTIFICATION_TYPE,
                                code: CUSTOMER_BATCH_IMPORT_FAIL_CODE,
                                description: string.Format(
                                    CUSTOMER_BATCH_IMPORT_FAIL_MESSAGE,
                                    i,
                                    _jsonSerializer.SerializeToJson(item)
                                )
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
