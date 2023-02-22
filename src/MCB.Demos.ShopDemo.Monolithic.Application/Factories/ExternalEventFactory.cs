using MCB.Core.Domain.Entities.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRemoved;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerImported;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductRemoved;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported;
using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Customers.CustomerRemoved;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Customers.CustomerImported;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductRemoved;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductImported;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Factories;

public class ExternalEventFactory
    : IExternalEventFactory
{
    // Constants
    public const string EXTERNAL_EVENT_FACTORY_DOMAIN_EVENT_NOT_MAPPER_MESSAGE_TEMPLATE = "ExternalEventFactory - Domain Event not mapped [{0}]";

    // Public Methods
    public EventBase? Create((IAdapter adapter, IDomainEvent domainEvent) parameter)
    {
        // Customer
        if (parameter.domainEvent is CustomerImportedDomainEvent customerImportedDomainEvent)
            return parameter.adapter.Adapt<CustomerImportedEvent>(customerImportedDomainEvent);
        else if (parameter.domainEvent is CustomerRemovedDomainEvent customerRemovedDomainEvent)
            return parameter.adapter.Adapt<CustomerRemovedEvent>(customerRemovedDomainEvent);

        // Product
        else if (parameter.domainEvent is ProductImportedDomainEvent productImportedDomainEvent)
            return parameter.adapter.Adapt<ProductImportedEvent>(productImportedDomainEvent);
        else if (parameter.domainEvent is ProductRemovedDomainEvent productRemovedDomainEvent)
            return parameter.adapter.Adapt<ProductRemovedEvent>(productRemovedDomainEvent);

        else
            throw new InvalidOperationException(
                string.Format(EXTERNAL_EVENT_FACTORY_DOMAIN_EVENT_NOT_MAPPER_MESSAGE_TEMPLATE, parameter.domainEvent.GetType().FullName)
            );
    }
}