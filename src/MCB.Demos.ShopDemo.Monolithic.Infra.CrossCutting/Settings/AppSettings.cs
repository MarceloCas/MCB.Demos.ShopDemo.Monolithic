using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.LoggingConfig;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.PostgreSqlConfig;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RedisConfig;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;

public class AppSettings
{
    // Properties
    public string ApplicationName { get; set; } = null!;
    public string ApplicationVersion { get; set; } = null!;
    public Redis Redis { get; set; } = null!;
    public PostgreSql PostgreSql { get; set; } = null!;
    public Models.RabbitMqConfig.RabbitMq RabbitMq { get; set; } = null!;
    public OpenTelemetry OpenTelemetry { get; set; } = null!;
    public Logging Logging { get; set; } = null!;
    public Consul Consul { get; set; } = null!;

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        var typeFullName = typeof(AppSettings).FullName;

        if (string.IsNullOrEmpty(ApplicationName))
            messageCollection.Add($"{typeFullName}.{nameof(ApplicationName)} cannot be null");

        if (string.IsNullOrEmpty(ApplicationVersion))
            messageCollection.Add($"{typeFullName}.{nameof(ApplicationVersion)} cannot be null");

        if (Redis is null)
            messageCollection.Add($"{typeFullName}.{nameof(Redis)} cannot be null");
        else
            messageCollection.AddRange(TtlSeconds.Validate().Messages);

        if (PostgreSql is null)
            messageCollection.Add($"{typeFullName}.{nameof(PostgreSql)} cannot be null");
        else
            messageCollection.AddRange(PostgreSql.Validate().Messages);

        if (RabbitMq is null)
            messageCollection.Add($"{typeFullName}.{nameof(RabbitMq)} cannot be null");
        else
            messageCollection.AddRange(RabbitMq.Validate().Messages);

        if (OpenTelemetry is null)
            messageCollection.Add($"{typeFullName}.{nameof(OpenTelemetry)} cannot be null");
        else
            messageCollection.AddRange(OpenTelemetry.Validate().Messages);

        if (Logging is null)
            messageCollection.Add($"{typeFullName}.{nameof(Logging)} cannot be null");
        else
            messageCollection.AddRange(Logging.Validate().Messages);

        if (Consul is null)
            messageCollection.Add($"{typeFullName}.{nameof(Consul)} cannot be null");
        else
            messageCollection.AddRange(Consul.Validate().Messages);

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}