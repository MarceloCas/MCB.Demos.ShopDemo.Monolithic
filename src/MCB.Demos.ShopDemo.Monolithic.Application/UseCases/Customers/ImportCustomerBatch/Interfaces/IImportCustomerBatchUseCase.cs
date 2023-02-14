using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomerBatch.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomerBatch.Interfaces;
public interface IImportCustomerBatchUseCase
    : IUseCase<ImportCustomerBatchUseCaseInput, int>
{
}
