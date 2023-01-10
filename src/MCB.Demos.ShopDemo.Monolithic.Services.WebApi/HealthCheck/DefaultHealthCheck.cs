using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using System.Text.Json;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;

public class DefaultHealthCheck
    : IHealthCheck
{
    // Constants
    public const string POSTGRESQL_SERVICE_NAME = "PostgreSQL";
    public const string REDIS_SERVICE_NAME = "Redis";

    //Fields
    private readonly AppSettings _appSettings;
    private static JsonSerializerOptions _jsonSerializeOptions = new()
    {
        PropertyNameCaseInsensitive = false
    };

    // Constructors
    public DefaultHealthCheck(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    // Public Methods
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var serviceStatusDictionary = new Dictionary<string, object>();

        // Validate PostgreSql
        var postgreSqlStatus = await ValidatePostgreSqlConnectionAsync(_appSettings.PostgreSql.ConnectionString, cancellationToken);
        serviceStatusDictionary.Add(postgreSqlStatus.Name, postgreSqlStatus.Status);
        // Validate Redis
        var redisStatus = await ValidateRedisConnectionAsync(_appSettings.Redis.ConnectionString, cancellationToken);
        serviceStatusDictionary.Add(redisStatus.Name, redisStatus.Status);

        var isHealthy = !serviceStatusDictionary.Any(q => (ServiceStatus)q.Value == ServiceStatus.Unhealthy);

        return isHealthy
            ? HealthCheckResult.Healthy(data: serviceStatusDictionary)
            : new HealthCheckResult(status: context.Registration.FailureStatus, data: serviceStatusDictionary);
    }
    public static Task WriteReport(HttpContext httpContext, HealthReport healthReport)
    {
        if (healthReport.Entries.Count == 0)
            return httpContext.Response.WriteAsJsonAsync(new ServiceReport(date: DateTime.UtcNow, null), _jsonSerializeOptions);


        var serviceReportItemCollection = new List<ServiceReportItem>(capacity: healthReport.Entries.Count);

        foreach (var entry in healthReport.Entries)
        {
            if (entry.Value.Data.Count == 0)
                continue;

            var serviceCollection = new List<Service>(capacity: entry.Value.Data.Count);
            foreach (var (dataKey, dataValue) in entry.Value.Data)
            {
                if (dataValue is ServiceStatus serviceStatus)
                    serviceCollection.Add(new Service(dataKey, serviceStatus));
                else
                    serviceCollection.Add(new Service(dataKey, 0));
            }

            serviceReportItemCollection.Add(
                new ServiceReportItem(
                    entryName: entry.Key,
                    serviceCollection: serviceCollection
                )
            );

        }

        return httpContext.Response.WriteAsJsonAsync(new ServiceReport(date: DateTime.UtcNow, serviceReportItemCollection), _jsonSerializeOptions);
    }

    // Private Methods
    private static async Task<Service> ValidatePostgreSqlConnectionAsync(string connectionString, CancellationToken cancellationToken)
    {
        var connection = new Npgsql.NpgsqlConnection(connectionString);

        try
        {
            await connection.OpenAsync(cancellationToken);
            return new Service(POSTGRESQL_SERVICE_NAME, ServiceStatus.Healthy);
        }
        catch (Exception)
        {
            return new Service(POSTGRESQL_SERVICE_NAME, ServiceStatus.Unhealthy);
        }
    }
    private static async Task<Service> ValidateRedisConnectionAsync(string connectionString, CancellationToken cancellationToken)
    {
        try
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);

            if(connection.IsConnected)
                return new Service(REDIS_SERVICE_NAME, ServiceStatus.Healthy);
            else
                return new Service(REDIS_SERVICE_NAME, ServiceStatus.Unhealthy);
        }
        catch (Exception)
        {
            return new Service(REDIS_SERVICE_NAME, ServiceStatus.Unhealthy);
        }
    }
}