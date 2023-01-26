namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;
public class Consul
{
    public string Address { get; set; } = null!;
    public int RefreshIntervalInSeconds { get; set; }
}
