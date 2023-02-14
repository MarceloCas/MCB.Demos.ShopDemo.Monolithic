namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Customers.ValidateImportCustomerBatch.Responses;
public record ValidateImportCustomerBatchUseCaseResponse
{
    // Properties
    public bool Success { get; set; }
    public IEnumerable<ValidateImportCustomerBatchUseCaseResponseItem> ItemCollection { get; set; }

    // Constructors
    public ValidateImportCustomerBatchUseCaseResponse(IEnumerable<ValidateImportCustomerBatchUseCaseResponseItem> itemCollection)
    {
        ItemCollection = itemCollection;
        Success = !itemCollection.Any(q => !q.Success);
    }
}
