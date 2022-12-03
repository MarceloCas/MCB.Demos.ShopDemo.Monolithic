using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;

public class AppSettings
{
    public Redis Redis { get; set; }
    public MongoDb MongoDb { get; set; }

    public AppSettings()
    {
        Redis = new Redis();
        MongoDb = new MongoDb();

    }
}