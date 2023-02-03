using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;

public abstract class UseCaseBase<TUseCaseInput, TUseCaseOutput>
    : IUseCase<TUseCaseInput, TUseCaseOutput>
    where TUseCaseInput : UseCaseInputBase
{
    // Constants
    public const string USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_CODE = "USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_CODE";
    public const string USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_DESCRIPTION = "Use case input cannot be null|UseCaseInputType:{0}";
    public const NotificationType USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_NOTIFICATION_TYPE = NotificationType.Error;

    public const string DOMAIN_EVENT_CANNOT_BE_NULL = "Domain event cannot be null";
    public const string EXTERNAL_EVENT_CANNOT_BE_NULL = "External event cannot be null";

    // Fields
    private readonly IDomainEventSubscriber _domainEventSubscriber;
    private readonly IExternalEventFactory _externalEventFactory;
    private readonly IEventsExchangeRabbitMqPublisher _eventsExchangeRabbitMqPublisher;

    // Properties
    protected INotificationPublisher NotificationPublisher { get; }
    protected ITraceManager TraceManager { get; }
    protected IAdapter Adapter { get; }
    protected IUnitOfWork UnitOfWork { get; }

    // Constructors
    protected UseCaseBase(
        IDomainEventSubscriber domainEventSubscriber,
        INotificationPublisher notificationPublisher,
        IEventsExchangeRabbitMqPublisher eventsExchangeRabbitMqPublisher,
        IExternalEventFactory externalEventFactory,
        ITraceManager traceManager,
        IAdapter adapter,
        IUnitOfWork unitOfWork
    )
    {
        _domainEventSubscriber = domainEventSubscriber;
        _externalEventFactory = externalEventFactory;
        _eventsExchangeRabbitMqPublisher = eventsExchangeRabbitMqPublisher;

        NotificationPublisher = notificationPublisher;
        TraceManager = traceManager;
        Adapter = adapter;
        UnitOfWork = unitOfWork;
    }

    // Public Methods
    public async Task<(bool Success, TUseCaseOutput? Output)> ExecuteAsync(TUseCaseInput? useCaseInput, CancellationToken cancellationToken)
    {
        if (!await CheckIfUseCaseInputIsNullAndSendNotification(useCaseInput, cancellationToken))
            return default;

        var useCaseOutput = await ExecuteInternalAsync(useCaseInput!, cancellationToken);

        if (!useCaseOutput.Success)
            return default;

        await PublishDomainEventsToExternalBusAsync(cancellationToken);

        return useCaseOutput;
    }

    // Protected Abstract Methods
    protected abstract Task<(bool Success, TUseCaseOutput? Output)> ExecuteInternalAsync(TUseCaseInput useCaseInput, CancellationToken cancellationToken);

    // Private Methods
    private async Task<bool> CheckIfUseCaseInputIsNullAndSendNotification(TUseCaseInput? useCaseInput, CancellationToken cancellationToken)
    {
        if (useCaseInput is not null)
            return true;

        await NotificationPublisher.PublishNotificationAsync(
            new Notification(
                notificationType: USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_NOTIFICATION_TYPE,
                code: USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_CODE,
                description: string.Format(USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_DESCRIPTION, typeof(TUseCaseInput).Name)
            ),
            cancellationToken
        );

        return false;
    }
    private async Task<bool> PublishDomainEventsToExternalBusAsync(CancellationToken cancellationToken)
    {
        foreach (var domainEventBase in _domainEventSubscriber.DomainEventCollection)
        {
            if (domainEventBase is null)
                throw new InvalidOperationException(DOMAIN_EVENT_CANNOT_BE_NULL);

            var externalEvent = _externalEventFactory.Create((Adapter, domainEventBase));
            if(externalEvent is null)
                throw new InvalidOperationException(EXTERNAL_EVENT_CANNOT_BE_NULL);

            await _eventsExchangeRabbitMqPublisher.PublishAsync(externalEvent, cancellationToken);
        }

        return true;
    }
}