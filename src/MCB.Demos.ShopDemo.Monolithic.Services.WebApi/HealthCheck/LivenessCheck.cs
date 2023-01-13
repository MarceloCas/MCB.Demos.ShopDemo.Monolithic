using MCB.Core.Infra.CrossCutting.RabbitMq.Connection.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;

public class LivenessCheck
    : IHealthCheck
{
    // Constants
    public const string POSTGRESQL_SERVICE_NAME = "PostgreSQL";
    public const string REDIS_SERVICE_NAME = "Redis";
    public const string RABBIT_MQ_SERVICE_NAME = "RabbitMq";

    //Fields
    private readonly AppSettings _appSettings;
    private readonly IRabbitMqConnection _rabbitMqConnection;

    // Constructors
    public LivenessCheck(
        AppSettings appSettings,
        IRabbitMqConnection rabbitMqConnection
    )
    {
        _appSettings = appSettings;
        _rabbitMqConnection = rabbitMqConnection;
    }

    // Public Methods
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var serviceStatusDictionary = new Dictionary<string, object>();

        // Validate PostgreSql
        var postgreSqlStatus = await ValidatePostgreSqlConnectionAsync(_appSettings.PostgreSql.ConnectionString, cancellationToken);
        serviceStatusDictionary.Add(postgreSqlStatus.Name, postgreSqlStatus.Status);
        // Validate Redis
        var redisStatus = await ValidateRedisConnectionAsync(_appSettings.Redis.ConnectionString);
        serviceStatusDictionary.Add(redisStatus.Name, redisStatus.Status);
        // Validate RabbitMq
        var rabbitMqStatus = ValidateRabbitMqConnection(_rabbitMqConnection);
        serviceStatusDictionary.Add(rabbitMqStatus.Name, rabbitMqStatus.Status);

        var isHealthy = !serviceStatusDictionary.Any(q => (ServiceStatus)q.Value == ServiceStatus.Unhealthy);

        return isHealthy
            ? HealthCheckResult.Healthy(data: serviceStatusDictionary)
            : new HealthCheckResult(status: context.Registration.FailureStatus, data: serviceStatusDictionary);
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
    private static async Task<Service> ValidateRedisConnectionAsync(string connectionString)
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
    private static Service ValidateRabbitMqConnection(IRabbitMqConnection rabbitMqConnection)
    {
        if (rabbitMqConnection.IsOpen)
            return new Service(RABBIT_MQ_SERVICE_NAME, ServiceStatus.Healthy);
        else
            return new Service(RABBIT_MQ_SERVICE_NAME, ServiceStatus.Unhealthy);
    }
}