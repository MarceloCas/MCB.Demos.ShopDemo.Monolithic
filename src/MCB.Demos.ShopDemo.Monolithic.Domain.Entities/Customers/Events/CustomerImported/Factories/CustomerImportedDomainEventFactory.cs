using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerImported.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Events.CustomerImported.Factories;

public class CustomerImportedDomainEventFactory
    : DomainEventFactoryBase,
    ICustomerImportedDomainEventFactory
{
    // Constructors
    public CustomerImportedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public CustomerImportedDomainEvent Create((Customer Customer, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<CustomerImportedDomainEvent>();

        return new CustomerImportedDomainEvent(
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