using MCB.Core.Domain.Entities.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;
using MCB.Demos.ShopDemo.Monolithic.Messages.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;

public interface IExternalEventFactory
    : IFactoryWithParameter<EventBase?, (IAdapter adapter, IDomainEvent domainEvent)>
{
}