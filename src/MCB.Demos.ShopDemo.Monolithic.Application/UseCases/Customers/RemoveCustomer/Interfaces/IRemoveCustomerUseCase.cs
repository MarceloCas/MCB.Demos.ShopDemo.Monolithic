using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.RemoveCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.RemoveCustomer.Interfaces;
public interface IRemoveCustomerUseCase
    : IUseCase<RemoveCustomerUseCaseInput, Customer>
{
}
