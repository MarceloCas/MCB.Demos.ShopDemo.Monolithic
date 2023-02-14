using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Interfaces;
public interface IImportProductBatchUseCase
    : IUseCase<ImportProductBatchUseCaseInput, int>
{
}

