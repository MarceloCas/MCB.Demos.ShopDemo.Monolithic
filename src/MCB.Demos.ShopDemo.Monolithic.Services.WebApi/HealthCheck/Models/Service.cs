using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models.Enums;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models;

public readonly struct Service
{
    public string Name { get; }
    public ServiceStatus Status { get; }
    public string StatusDescription => Status.ToString();

    public Service(string name, ServiceStatus status)
    {
        Name = name;
        Status = status;
    }
}
