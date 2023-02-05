namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Responses;
public record ValidateImportCustomerBatchUseCaseResponse
{
    // Properties
    public IEnumerable<ValidateImportCustomerBatchUseCaseResponseItem> ItemCollection { get; set; }
    public bool Success { get; set; }

    // Constructors
    public ValidateImportCustomerBatchUseCaseResponse(IEnumerable<ValidateImportCustomerBatchUseCaseResponseItem> itemCollection)
    {
        ItemCollection = itemCollection;
        Success = !itemCollection.Any(q => !q.Success);
    }
}
