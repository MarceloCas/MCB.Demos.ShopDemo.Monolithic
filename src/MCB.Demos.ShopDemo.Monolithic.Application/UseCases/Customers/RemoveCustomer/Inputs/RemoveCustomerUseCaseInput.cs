using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.RemoveCustomer.Inputs;
public record RemoveCustomerUseCaseInput
    : UseCaseInputBase
{
    // Properties
    public string? Email { get; set; }
}
