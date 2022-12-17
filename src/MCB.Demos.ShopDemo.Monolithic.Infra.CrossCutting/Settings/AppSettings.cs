using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;

public class AppSettings
{
    public Redis Redis { get; set; }
    public PostgreSql PostgreSql { get; set; }

    public AppSettings()
    {
        Redis = new Redis();
        PostgreSql = new PostgreSql();

    }
}