namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RabbitMqConfig;

public class Connection
{
    public string ClientProvidedName { get; set; } = null!;
    public string HostName { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string VirtualHost { get; set; } = null!;
    public bool DispatchConsumersAsync { get; set; }
    public bool AutoConnect { get; set; }
    public bool AutomaticRecoveryEnabled { get; set; }
    public int NetworkRecoveryIntervalSeconds { get; set; }
    public bool TopologyRecoveryEnabled { get; set; }
    public int RequestedHeartbeatSeconds { get; set; }
}
