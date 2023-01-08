namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RedisConfig;

public class Redis
{
    public string ConnectionString { get; set; } = null!;
    public TTLSeconds TTLSeconds { get; set; } = null!;
}