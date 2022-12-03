namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

public class MongoDb
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }

    public MongoDb()
    {
        ConnectionString = string.Empty;
        DatabaseName = string.Empty;
    }
}