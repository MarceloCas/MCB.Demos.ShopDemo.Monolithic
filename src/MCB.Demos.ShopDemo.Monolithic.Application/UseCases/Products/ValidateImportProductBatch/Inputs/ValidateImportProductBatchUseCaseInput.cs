using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Inputs;
public record ValidateImportProductBatchUseCaseInput
    : UseCaseInputBase
{
    public ValidateImportProductBatchUseCaseInputItem[] Items { get; set; }

    public ValidateImportProductBatchUseCaseInput()
    {
        Items = Array.Empty<ValidateImportProductBatchUseCaseInputItem>();
    }
}

