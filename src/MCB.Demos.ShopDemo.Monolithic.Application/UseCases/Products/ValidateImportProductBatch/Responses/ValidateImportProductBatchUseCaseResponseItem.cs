using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ValidateImportProductBatch.Responses;
public record ValidateImportProductBatchUseCaseResponseItem
{
    // Properties
    public int Index { get; set; }
    public string? Code { get; set; }
    public bool Success { get; set; }
    public IEnumerable<Notification>? NotificationCollection { get; set; }

    // Constructors
    public ValidateImportProductBatchUseCaseResponseItem(int index, string? code, bool success, IEnumerable<Notification>? notificationCollection)
    {
        Index = index;
        Code = code;
        Success = success;
        NotificationCollection = notificationCollection;
    }
}
