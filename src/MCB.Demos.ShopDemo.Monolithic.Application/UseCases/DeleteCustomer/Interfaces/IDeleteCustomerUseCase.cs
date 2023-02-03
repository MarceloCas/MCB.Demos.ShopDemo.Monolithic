using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.DeleteCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.DeleteCustomer.Interfaces;
public interface IDeleteCustomerUseCase
    : IUseCase<DeleteCustomerUseCaseInput, Customer>
{
}
