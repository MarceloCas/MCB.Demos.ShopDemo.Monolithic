using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ImportCustomer.Inputs;

public record ImportCustomerUseCaseInput
    : UseCaseInputBase
{
    // Properties
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Email { get; set; }
}