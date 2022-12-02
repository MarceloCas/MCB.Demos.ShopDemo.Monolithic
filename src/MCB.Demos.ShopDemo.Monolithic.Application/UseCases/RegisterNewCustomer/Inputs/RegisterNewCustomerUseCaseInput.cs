using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;

public record RegisterNewCustomerUseCaseInput
    : UseCaseInputBase
{
    // Properties
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Email { get; set; }
}