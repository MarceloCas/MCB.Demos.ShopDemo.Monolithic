using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Inputs;
public record ImportProductBatchUseCaseInput
    : UseCaseInputBase
{
    public ImportProductBatchUseCaseInputItem[] Items { get; set; }

    public ImportProductBatchUseCaseInput()
    {
        Items = Array.Empty<ImportProductBatchUseCaseInputItem>();
    }
}

