using MCB.Core.Domain.Entities.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRegistered;
using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.CustomerDeleted;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.CustomerRegistered;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Factories;

public class ExternalEventFactory
    : IExternalEventFactory
{
    // Constants
    public const string EXTERNAL_EVENT_FACTORY_DOMAIN_EVENT_NOT_MAPPER_MESSAGE_TEMPLATE = "ExternalEventFactory - Domain Event not mapped [{0}]";

    // Public Methods
    public EventBase? Create((IAdapter adapter, IDomainEvent domainEvent) parameter)
    {
        if (parameter.domainEvent is CustomerRegisteredDomainEvent customerRegisteredDomainEvent)
            return parameter.adapter.Adapt<CustomerRegisteredEvent>(customerRegisteredDomainEvent);
        else if (parameter.domainEvent is CustomerDeletedDomainEvent customerDeletedDomainEvent)
            return parameter.adapter.Adapt<CustomerDeletedEvent>(customerDeletedDomainEvent);
        else
            throw new InvalidOperationException(
                string.Format(EXTERNAL_EVENT_FACTORY_DOMAIN_EVENT_NOT_MAPPER_MESSAGE_TEMPLATE, parameter.domainEvent.GetType().FullName)
            );
    }
}