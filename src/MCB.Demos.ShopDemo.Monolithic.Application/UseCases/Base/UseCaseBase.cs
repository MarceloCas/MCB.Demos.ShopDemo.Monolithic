using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;

public abstract class UseCaseBase<TUseCaseInput>
    : IUseCase<TUseCaseInput>
    where TUseCaseInput : UseCaseInputBase
{
    // Fields
    private readonly IDomainEventSubscriber _domainEventSubscriber;
    private readonly IExternalEventFactory _externalEventFactory;

    // Properties
    protected IAdapter Adapter { get; }

    // Constructors
    protected UseCaseBase(
        IDomainEventSubscriber domainEventSubscriber,
        IExternalEventFactory externalEventFactory,
        IAdapter adapter
    )
    {
        _domainEventSubscriber = domainEventSubscriber;
        _externalEventFactory = externalEventFactory;
        Adapter = adapter;
    }

    // Public Methods
    public async Task<bool> ExecuteAsync(TUseCaseInput useCaseInput, CancellationToken cancellationToken)
    {
        if (!await ExecuteInternalAsync(useCaseInput, cancellationToken))
            return false;

        return await PublishDomainEventsToExternalBusAsync(cancellationToken);
    }

    // Protected Abstract Methods
    protected abstract Task<bool> ExecuteInternalAsync(TUseCaseInput useCaseInput, CancellationToken cancellationToken);

    // Private Methods
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