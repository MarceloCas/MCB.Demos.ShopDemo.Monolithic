using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.DeleteCustomer.Inputs;
public record DeleteCustomerUseCaseInput
    : UseCaseInputBase
{
    // Properties
    public string? Email { get; set; }
}
