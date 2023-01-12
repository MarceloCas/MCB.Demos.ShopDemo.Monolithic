namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RabbitMqConfig;

public class RabbitMq
{
    public Connection Connection { get; set; } = null!;
    public EventsExchange EventsExchange { get; set; } = null!;
    public ResiliencePolicy ResiliencePolicy { get; set; } = null!;
}
