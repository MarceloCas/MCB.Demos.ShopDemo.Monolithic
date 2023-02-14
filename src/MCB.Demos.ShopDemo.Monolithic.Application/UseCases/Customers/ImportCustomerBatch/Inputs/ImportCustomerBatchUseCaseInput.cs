using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomerBatch.Inputs;

public record ImportCustomerBatchUseCaseInput
    : UseCaseInputBase
{
    public ImportCustomerBatchUseCaseInputItem[] Items { get; set; }

    public ImportCustomerBatchUseCaseInput()
    {
        Items = Array.Empty<ImportCustomerBatchUseCaseInputItem>();
    }
}
