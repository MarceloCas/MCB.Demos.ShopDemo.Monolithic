namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RabbitMqConfig;

public class EventsExchange
{
    public string Name { get; set; } = null!;
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public IDictionary<string, object>? Arguments { get; set; }
}
