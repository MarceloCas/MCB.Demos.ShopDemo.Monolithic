namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Inputs;
public record ValidateImportCustomerBatchUseCaseInputItem
{
    // Properties
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Email { get; set; }
}
