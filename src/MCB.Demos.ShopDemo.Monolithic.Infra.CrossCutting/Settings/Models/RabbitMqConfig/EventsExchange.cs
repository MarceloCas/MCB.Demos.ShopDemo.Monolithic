namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RabbitMqConfig;

public class EventsExchange
{
    // Properties
    public string Name { get; set; } = null!;
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public IDictionary<string, object>? Arguments { get; set; }

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            messageCollection.Add($"{typeof(EventsExchange).FullName}.{nameof(Name)} cannot be null");

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
