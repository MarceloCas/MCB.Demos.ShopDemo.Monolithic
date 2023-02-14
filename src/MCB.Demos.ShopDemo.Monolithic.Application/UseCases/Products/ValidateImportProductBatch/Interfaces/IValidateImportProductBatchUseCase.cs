using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Responses;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Interfaces;
public interface IValidateImportProductBatchUseCase
    : IUseCase<ValidateImportProductBatchUseCaseInput, ValidateImportProductBatchUseCaseResponse>
{
}

