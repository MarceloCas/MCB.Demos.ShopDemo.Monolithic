using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Factory;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;

public interface ICustomerFactory
    : IFactory<Customer>
{
}