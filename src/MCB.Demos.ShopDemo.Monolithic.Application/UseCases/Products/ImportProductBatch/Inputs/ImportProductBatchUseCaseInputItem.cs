namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProductBatch.Inputs;
public record ImportProductBatchUseCaseInputItem
{
    // Properties
    public string? Code { get; set; }
    public string? Description { get; set; }
}