using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories.Interfaces;
public interface IOrderFactory
    : IFactory<Order>
{
}
