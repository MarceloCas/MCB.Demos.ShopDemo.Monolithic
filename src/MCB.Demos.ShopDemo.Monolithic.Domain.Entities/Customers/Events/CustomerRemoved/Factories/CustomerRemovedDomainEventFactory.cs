using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRemoved.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerRemoved.Factories;
public class CustomerRemovedDomainEventFactory
    : DomainEventFactoryBase,
    ICustomerRemovedDomainEventFactory
{
    // Constructors
    public CustomerRemovedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public CustomerRemovedDomainEvent? Create((Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<CustomerRemovedDomainEvent>();

        return new CustomerRemovedDomainEvent(
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
