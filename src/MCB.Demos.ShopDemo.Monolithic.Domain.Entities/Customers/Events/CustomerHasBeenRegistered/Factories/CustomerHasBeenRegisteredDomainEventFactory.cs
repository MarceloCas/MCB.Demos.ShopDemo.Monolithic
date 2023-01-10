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
    public CustomerHasBeenRegisteredDomainEvent Create((Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<CustomerHasBeenRegisteredDomainEvent>();

        return new CustomerHasBeenRegisteredDomainEvent(
            correlationId: parameter.CorrelationId,
            id: id,
            tenantId: parameter.Customer.TenantId,
            timestamp: timestamp,
            executionUser: parameter.ExecutionUser,
            sourcePlatform: parameter.SourcePlatform,
            domainEventType: domainEventType,
            parameter.Customer
        );
    }
}