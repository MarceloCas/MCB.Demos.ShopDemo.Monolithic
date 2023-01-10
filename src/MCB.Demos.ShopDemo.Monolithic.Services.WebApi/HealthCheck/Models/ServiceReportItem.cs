using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models.Enums;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models;

public readonly struct ServiceReportItem
{
    // Properties
    public string EntryName { get; }
    public ServiceStatus Status { get; }
    public string StatusDescription => Status.ToString();
    public IEnumerable<Service>? ServiceCollection { get; }

    // Constructors
    public ServiceReportItem(string entryName, IEnumerable<Service>? serviceCollection)
    {
        EntryName = entryName;
        ServiceCollection = serviceCollection;

        if (serviceCollection is null || !serviceCollection.Any())
            Status = ServiceStatus.Healthy;
        else if (serviceCollection.Any(q => q.Status == ServiceStatus.Unhealthy))
            Status = ServiceStatus.Unhealthy;
        else if (serviceCollection.Any(q => q.Status == ServiceStatus.Partial))
            Status = ServiceStatus.Partial;
        else
            Status = ServiceStatus.Healthy;
    }
}
