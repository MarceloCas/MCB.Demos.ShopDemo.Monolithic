namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;

public class PostgreSql
{
    public string ConnectionString { get; set; }

    public PostgreSql()
    {
        ConnectionString = string.Empty;
    }
}