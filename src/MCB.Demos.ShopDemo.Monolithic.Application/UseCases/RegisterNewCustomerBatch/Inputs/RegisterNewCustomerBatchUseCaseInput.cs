using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Inputs;

public record RegisterNewCustomerBatchUseCaseInput
    : UseCaseInputBase
{
    public RegisterNewCustomerBatchUseCaseInputItem[] Items { get; set; }

    public RegisterNewCustomerBatchUseCaseInput()
    {
        Items = Array.Empty<RegisterNewCustomerBatchUseCaseInputItem>();
    }
}
