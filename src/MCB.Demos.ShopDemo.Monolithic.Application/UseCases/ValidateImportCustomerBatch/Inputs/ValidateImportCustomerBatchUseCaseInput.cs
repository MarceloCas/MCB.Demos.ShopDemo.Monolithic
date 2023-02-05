using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Inputs;
public record ValidateImportCustomerBatchUseCaseInput
    : UseCaseInputBase
{
    public ValidateImportCustomerBatchUseCaseInputItem[] Items { get; set; }

    public ValidateImportCustomerBatchUseCaseInput()
    {
        Items = Array.Empty<ValidateImportCustomerBatchUseCaseInputItem>();
    }
}
