namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

public class Redis
{
    public string ConnectionString { get; set; }

    public Redis()
    {
        ConnectionString = string.Empty;
    }
}