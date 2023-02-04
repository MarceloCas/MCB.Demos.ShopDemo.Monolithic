namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RabbitMqConfig;

public class RabbitMq
{
    // Properties
    public Connection Connection { get; set; } = null!;
    public EventsExchange EventsExchange { get; set; } = null!;
    public ResiliencePolicy ResiliencePolicy { get; set; } = null!;

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        var typeFullName = typeof(RabbitMq).FullName;

        if (Connection is null)
            messageCollection.Add($"{typeFullName}.{nameof(Connection)} cannot be null");
        else
            messageCollection.AddRange(Connection.Validate().Messages);

        if (EventsExchange is null)
            messageCollection.Add($"{typeFullName}.{nameof(EventsExchange)} cannot be null");
        else
            messageCollection.AddRange(EventsExchange.Validate().Messages);

        if (ResiliencePolicy is null)
            messageCollection.Add($"{typeFullName}.{nameof(ResiliencePolicy)} cannot be null");
        else
            messageCollection.AddRange(ResiliencePolicy.Validate().Messages);

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
