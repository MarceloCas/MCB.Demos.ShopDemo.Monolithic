namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RabbitMqConfig;

public class Connection
{
    // Properties
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

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        var typeFullName = typeof(Connection).FullName;

        if (string.IsNullOrWhiteSpace(ClientProvidedName))
            messageCollection.Add($"{typeFullName}.{nameof(ClientProvidedName)} cannot be null");

        if (string.IsNullOrWhiteSpace(HostName))
            messageCollection.Add($"{typeFullName}.{nameof(HostName)} cannot be null");

        if (string.IsNullOrWhiteSpace(Username))
            messageCollection.Add($"{typeFullName}.{nameof(Username)} cannot be null");

        if (string.IsNullOrWhiteSpace(Password))
            messageCollection.Add($"{typeFullName}.{nameof(Password)} cannot be null");

        if (string.IsNullOrWhiteSpace(VirtualHost))
            messageCollection.Add($"{typeFullName}.{nameof(VirtualHost)} cannot be null");

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
