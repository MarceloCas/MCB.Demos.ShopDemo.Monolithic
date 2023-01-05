namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Models;

public class RedisOptions
{
    // Properties
    public string ConnectionString { get; }

    // Constructors
    public RedisOptions(string connectionString)
    {
        ConnectionString = connectionString;
    }
}