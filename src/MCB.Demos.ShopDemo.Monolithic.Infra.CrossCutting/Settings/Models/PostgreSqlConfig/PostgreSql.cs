namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.PostgreSqlConfig;

public class PostgreSql
{
    public string ConnectionString { get; set; } = null!;
    public ResiliencePolicy ResiliencePolicy { get; set; } = null!;
}