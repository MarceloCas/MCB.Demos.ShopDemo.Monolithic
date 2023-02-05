using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.ValidateImportCustomerBatch.Responses;
public record ValidateImportCustomerBatchUseCaseResponseItem
{
    // Properties
    public int Index { get; set; }
    public string? Email { get; set; }
    public bool Success { get; set; }
    public IEnumerable<Notification>? NotificationCollection { get; set; }

    // Constructors
    public ValidateImportCustomerBatchUseCaseResponseItem(int index, string? email, bool success, IEnumerable<Notification>? notificationCollection)
    {
        Index = index;
        Email = email;
        Success = success;
        NotificationCollection = notificationCollection;
    }
}
