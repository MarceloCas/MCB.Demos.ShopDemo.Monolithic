using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerDeleted.Factories;
public class CustomerDeletedDomainEventFactory
    : DomainEventFactoryBase,
    ICustomerDeletedDomainEventFactory
{
    // Constructors
    public CustomerDeletedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public CustomerDeletedDomainEvent? Create((Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<CustomerDeletedDomainEvent>();

        return new CustomerDeletedDomainEvent(
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
