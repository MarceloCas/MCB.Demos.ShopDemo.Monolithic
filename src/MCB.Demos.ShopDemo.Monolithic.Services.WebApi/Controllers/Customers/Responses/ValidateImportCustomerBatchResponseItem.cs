using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;

public record ValidateImportCustomerBatchResponseItem
{
    // Properties
    public int Index { get; set; }
    public string? Email { get; set; }
    public bool Success { get; set; }
    public IEnumerable<Notification> NotificationCollection { get; set; }

    public ValidateImportCustomerBatchResponseItem()
    {
        NotificationCollection = Enumerable.Empty<Notification>();
    }
}
