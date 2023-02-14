using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Responses;

public record ValidateImportProductBatchResponseItem
{
    // Properties
    public int Index { get; set; }
    public string? Code { get; set; }
    public bool Success { get; set; }
    public IEnumerable<Notification>? NotificationCollection { get; set; }
}
