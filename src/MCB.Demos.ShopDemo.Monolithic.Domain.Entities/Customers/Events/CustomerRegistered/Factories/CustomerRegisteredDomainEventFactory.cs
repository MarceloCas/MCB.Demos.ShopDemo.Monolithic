using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRegistered.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRegistered.Factories;

public class CustomerRegisteredDomainEventFactory
    : DomainEventFactoryBase,
    ICustomerRegisteredDomainEventFactory
{
    // Constructors
    public CustomerRegisteredDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public CustomerRegisteredDomainEvent Create((Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<CustomerRegisteredDomainEvent>();

        return new CustomerRegisteredDomainEvent(
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