using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ImportCustomer.Interfaces;

public interface IImportCustomerUseCase
    : IUseCase<ImportCustomerUseCaseInput>
{
}