using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.LoggingConfig;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.PostgreSqlConfig;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RedisConfig;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;

public class AppSettings
{
    public string ApplicationName { get; set; }
    public string ApplicationVersion { get; set; }
    public Redis Redis { get; set; }
    public PostgreSql PostgreSql { get; set; }
    public Models.RabbitMqConfig.RabbitMq RabbitMq { get; set; }
    public OpenTelemetry OpenTelemetry { get; set; }
    public Models.LoggingConfig.Logging Logging { get; set; }

    public AppSettings()
    {
        ApplicationName = string.Empty;
        ApplicationVersion = string.Empty;
        Redis = new Redis();
        PostgreSql = new PostgreSql();
        RabbitMq = new Models.RabbitMqConfig.RabbitMq();
        OpenTelemetry = new OpenTelemetry();
        Logging = new Logging();
    }
}