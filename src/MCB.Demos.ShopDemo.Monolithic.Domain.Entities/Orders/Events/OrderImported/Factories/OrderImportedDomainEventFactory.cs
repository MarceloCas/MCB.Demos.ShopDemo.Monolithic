using MCB.Core.Domain.Entities.DomainEntitiesBase.Events.Factories;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderImported.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Events.OrderImported.Factories;
public class OrderImportedDomainEventFactory
    : DomainEventFactoryBase,
    IOrderImportedDomainEventFactory
{
    // Constructors
    public OrderImportedDomainEventFactory(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public OrderImportedDomainEvent Create((Order Order, string ExecutionUser, string SourcePlatform, Guid CorrelationId) parameter)
    {
        var (id, timestamp, domainEventType) = GetBaseEventFields<OrderImportedDomainEvent>();

        return new OrderImportedDomainEvent(
            correlationId: parameter.CorrelationId,
            id: id,
            tenantId: parameter.Order.TenantId,
            timestamp: timestamp,
            executionUser: parameter.ExecutionUser,
            sourcePlatform: parameter.SourcePlatform,
            domainEventType: domainEventType,
            parameter.Order
        );
    }
}