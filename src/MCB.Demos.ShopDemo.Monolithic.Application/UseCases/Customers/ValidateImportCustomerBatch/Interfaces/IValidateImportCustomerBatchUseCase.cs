using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Responses;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Interfaces;
public interface IValidateImportCustomerBatchUseCase
    : IUseCase<ValidateImportCustomerBatchUseCaseInput, ValidateImportCustomerBatchUseCaseResponse>
{
}
