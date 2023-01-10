namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RedisConfig;

public class Redis
{
    public string ConnectionString { get; set; } = null!;
    public TtlSeconds TtlSeconds { get; set; } = null!;
}