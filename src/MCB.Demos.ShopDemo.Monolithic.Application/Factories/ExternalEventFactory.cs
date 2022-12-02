using MCB.Core.Domain.Entities.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered;
using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.CustomerHasBeenRegistered;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Factories;

public class ExternalEventFactory
    : IExternalEventFactory
{
    public EventBase? Create((IAdapter adapter, IDomainEvent domainEvent) parameter)
    {
        if (parameter.domainEvent is CustomerHasBeenRegisteredDomainEvent domainEvent)
            return parameter.adapter.Adapt<CustomerHasBeenRegisteredEvent>(domainEvent);
        else
            return null;
    }
}