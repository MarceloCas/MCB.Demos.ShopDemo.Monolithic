using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerHasBeenRegistered.Factories;

public class CustomerHasBeenRegisteredDomainEventFactory
    : DomainEventFactoryBase,
    ICustomerHasBeenRegisteredDomainEventFactory
{
    // Constructors
    public CustomerHasBeenRegisteredDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public CustomerHasBeenRegisteredDomainEvent Create((Customer customer, string executionUser, string sourcePlatform) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<CustomerHasBeenRegisteredDomainEvent>();

        return new CustomerHasBeenRegisteredDomainEvent(
            id: id,
            tenantId: parameter.customer.TenantId,
            timestamp: timestamp,
            executionUser: parameter.executionUser,
            sourcePlatform: parameter.sourcePlatform,
            domainEventType: domainEventType,
            parameter.customer
        );
    }
}