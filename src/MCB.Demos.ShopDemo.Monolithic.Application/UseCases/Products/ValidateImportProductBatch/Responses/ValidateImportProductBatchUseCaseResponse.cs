namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Responses;
public record ValidateImportProductBatchUseCaseResponse
{
    // Properties
    public bool Success { get; set; }
    public IEnumerable<ValidateImportProductBatchUseCaseResponseItem> ItemCollection { get; set; }

    // Constructors
    public ValidateImportProductBatchUseCaseResponse(IEnumerable<ValidateImportProductBatchUseCaseResponseItem> itemCollection)
    {
        ItemCollection = itemCollection;
        Success = !itemCollection.Any(q => !q.Success);
    }
}

