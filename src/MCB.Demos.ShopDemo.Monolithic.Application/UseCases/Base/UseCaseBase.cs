﻿using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;

public abstract class UseCaseBase<TUseCaseInput>
    : IUseCase<TUseCaseInput>
    where TUseCaseInput : UseCaseInputBase
{
    // Constants
    public const string USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_CODE = "USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_CODE";
    public const string USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_DESCRIPTION = "Use case input cannot be null|UseCaseInputType:{0}";
    public const NotificationType USE_CASE_INPUT_CANNOT_BE_NULL_ERROR_NOTIFICATION_TYPE = NotificationType.Error;

    // Fields
    private readonly IDomainEventSubscriber _domainEventSubscriber;
    private readonly IExternalEventFactory _externalEventFactory;

    // Properties
    protected INotificationPublisher NotificationPublisher { get; set; }
    protected IAdapter Adapter { get; }

    // Constructors
    protected UseCaseBase(
        INotificationPublisher notificationPublisher,
        IDomainEventSubscriber domainEventSubscriber,
        IExternalEventFactory externalEventFactory,
        IAdapter adapter
    )
    {
        _domainEventSubscriber = domainEventSubscriber;
        _externalEventFactory = externalEventFactory;

        NotificationPublisher = notificationPublisher;
        Adapter = adapter;
    }

    // Public Methods
    public async Task<bool> ExecuteAsync(TUseCaseInput? useCaseInput, CancellationToken cancellationToken)
    {
        if (!await CheckIfUseCaseInputIsNullAndSendNotification(useCaseInput, cancellationToken))
            return false;

        if (!await ExecuteInternalAsync(useCaseInput!, cancellationToken))
            return false;

        return await PublishDomainEventsToExternalBusAsync(cancellationToken);
    }

    // Protected Abstract Methods
    protected abstract Task<bool> ExecuteInternalAsync(TUseCaseInput useCaseInput, CancellationToken cancellationToken);

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
                continue;

            var externalEvent = _externalEventFactory.Create((Adapter, domainEventBase));
        }

        await Task.Delay(1);

        return true;
    }
}