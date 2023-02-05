using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Responses;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Interfaces;
public interface IValidateImportCustomerBatchUseCase
    : IUseCase<ValidateImportCustomerBatchUseCaseInput, ValidateImportCustomerBatchUseCaseResponse>
{
}
