using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomer.Interfaces;

public interface IImportCustomerUseCase
    : IUseCase<ImportCustomerUseCaseInput, Customer>
{
}