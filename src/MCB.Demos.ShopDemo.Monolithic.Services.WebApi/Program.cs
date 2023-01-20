using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience.Models;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Core.Infra.CrossCutting.RabbitMq.Connection.Interfaces;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;
using OpenTelemetry;
using Npgsql;
using OpenTelemetry.Metrics;
using System.Reflection.PortableExecutable;
using OpenTelemetry.Logs;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Logging;

var builder = WebApplication.CreateBuilder(args);

#region Config
var appSettings = builder.Configuration.Get<AppSettings>();

if (appSettings == null)
    throw new InvalidOperationException("AppSettings cannot be null");

builder.Services.AddSingleton(appSettings);
#endregion

#region Configure Service
builder.Services.AddOpenTelemetry()
    .ConfigureResource(builder => { })
    .WithTracing(builder => builder
        .AddHttpClientInstrumentation(options => { })
        .AddAspNetCoreInstrumentation(options => { })
        .AddSource(appSettings.ApplicationName)
        .SetResourceBuilder(
            ResourceBuilder
                .CreateDefault()
                .AddService(serviceName: appSettings.ApplicationName, serviceVersion: appSettings.ApplicationVersion)
        )
        .AddEntityFrameworkCoreInstrumentation(options => { })
        .AddRedisInstrumentation()
        .AddNpgsql(options => { })
        .AddOtlpExporter(options => options.Endpoint = new Uri(appSettings.OpenTelemetry.GrpcCollectorReceiverUrl))
    )
    .WithMetrics(builder => builder
        .AddMeter(appSettings.ApplicationName)
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter(options => options.Endpoint = new Uri(appSettings.OpenTelemetry.GrpcCollectorReceiverUrl))
)
.StartWithHost();

builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(
            serviceName: appSettings.ApplicationName,
            serviceVersion: appSettings.ApplicationVersion,
            serviceInstanceId: Environment.MachineName
        )
    );

    options
        .AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri(appSettings.OpenTelemetry.GrpcCollectorReceiverUrl);
        })
        .AddConsoleExporter()
        .AddProcessor(new OpenTelemetryLogGrayLogProcessor());
});
builder.Logging.AddFilter<OpenTelemetryLoggerProvider>("*", Enum.Parse<LogLevel>(appSettings.Logging.LogLevel.Default));

builder.Services.AddMcbDependencyInjection(dependencyInjectionContainer =>
    MCB.Demos.ShopDemo.Monolithic.Services.WebApi.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(
        applicationName: appSettings.ApplicationName,
        applicationVersion: appSettings.ApplicationVersion,
        dependencyInjectionContainer,
        adapterMapAction: typeAdapterConfig => AdapterConfig.Configure(dependencyInjectionContainer),
        appSettings!
    )
);

builder.Services.AddDbContextPool<DefaultEntityFrameworkDataContext>(
    options => options.UseNpgsql(
        appSettings!.PostgreSql.ConnectionString
    )
);
builder.Services.AddScoped<IEntityFrameworkDataContext>(serviceCollection =>
{
    var config = serviceCollection.GetService<IConfiguration>()!;

    return new DefaultEntityFrameworkDataContext(
        serviceCollection.GetService<ITraceManager>()!,
        config["PostgreSql:ConnectionString"]!,
        serviceCollection.GetService<IPostgreSqlResiliencePolicy>()!
    );
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MCB.Demos.ShopDemo.Monolithic.Services.WebApi",
        Description = "ShopDemo (Monolithic) Web Api",
        Contact = new OpenApiContact()
        {
            Name = "Marcelo Castelo Branco",
            Email = "marcelo.castelo@outlook.com",
            Url = new Uri("https://www.linkedin.com/in/marcelocastelobranco/")
        }
    });
});
builder.Services.AddHealthChecks()
    .AddCheck<StartupCheck>("/health/startup", tags: new[] { "startup" })
    .AddCheck<ReadinessCheck>("/health/readiness", tags: new[] { "readiness" })
    .AddCheck<LivenessCheck>("/health/liveness", tags: new[] { "liveness" });
#endregion

#region Configure Pipeline
var app = builder.Build();

app.UseMcbDependencyInjection();
app.UseMcbGlobalExceptionMiddleware();
app.UseMcbRequestCounterMetricMiddleware();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks(
    "/health/startup",
    options: new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = ReportWriter.WriteReport,
        Predicate = healthCheck => healthCheck.Tags.Contains("startup")
    }
);
app.MapHealthChecks(
    "/health/readiness",
    options: new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = ReportWriter.WriteReport,
        Predicate = healthCheck => healthCheck.Tags.Contains("readiness")
    }
);
app.MapHealthChecks(
    "/health/liveness",
    options: new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = ReportWriter.WriteReport,
        Predicate = healthCheck => healthCheck.Tags.Contains("liveness")
    }
);
#endregion

#region Startup
// Startup RabbitMQ
var dependencyInjectionContainer = app.Services.GetService<IDependencyInjectionContainer>()!;

var rabbitMqConnection = dependencyInjectionContainer.Resolve<IRabbitMqConnection>()!;
rabbitMqConnection.OpenConnection();
rabbitMqConnection.ExchangeDeclare(
    new RabbitMqExchangeConfig(
        ExchangeName: appSettings.RabbitMq.EventsExchange.Name,
        ExchangeType: RabbitMqExchangeType.Header,
        Durable: appSettings.RabbitMq.EventsExchange.Durable,
        AutoDelete: appSettings.RabbitMq.EventsExchange.AutoDelete,
        Arguments: null
    )
);

// Configure resilience policies
var postgreSqlResiliencePolicy = dependencyInjectionContainer.Resolve<IPostgreSqlResiliencePolicy>()!;
postgreSqlResiliencePolicy.Configure(() => new ResiliencePolicyConfig
{
    // Identification
    Name = appSettings.PostgreSql.ResiliencePolicy.Name,
    // Retry
    RetryMaxAttemptCount = appSettings.PostgreSql.ResiliencePolicy.RetryMaxAttemptCount,
    RetryAttemptWaitingTimeFunction = (attempt) => TimeSpan.FromMilliseconds(appSettings.PostgreSql.ResiliencePolicy.RetryAttemptWaitingTimeMilliseconds * attempt),
    OnRetryAditionalHandler = null,
    // Circuit Breaker
    CircuitBreakerWaitingTimeFunction = () => TimeSpan.FromSeconds(appSettings.PostgreSql.ResiliencePolicy.CircuitBreakerWaitingTimeSeconds),
    OnCircuitBreakerHalfOpenAditionalHandler = null,
    OnCircuitBreakerOpenAditionalHandler = null,
    OnCircuitBreakerCloseAditionalHandler = null,
    // Exceptions
    ExceptionHandleConfigArray = new[] {
        new Func<Exception, bool>(ex => ex.GetType() == typeof(Npgsql.NpgsqlException)),
        new Func<Exception, bool>(ex => ex.GetType() == typeof(Npgsql.PostgresException)),
    }
});

var redisResiliencePolicy = dependencyInjectionContainer.Resolve<IRedisResiliencePolicy>()!;
redisResiliencePolicy.Configure(() => new ResiliencePolicyConfig
{
    // Identification
    Name = appSettings.Redis.ResiliencePolicy.Name,
    // Retry
    RetryMaxAttemptCount = appSettings.Redis.ResiliencePolicy.RetryMaxAttemptCount,
    RetryAttemptWaitingTimeFunction = (attempt) => TimeSpan.FromMilliseconds(appSettings.Redis.ResiliencePolicy.RetryAttemptWaitingTimeMilliseconds * attempt),
    OnRetryAditionalHandler = null,
    // Circuit Breaker
    CircuitBreakerWaitingTimeFunction = () => TimeSpan.FromSeconds(appSettings.Redis.ResiliencePolicy.CircuitBreakerWaitingTimeSeconds),
    OnCircuitBreakerHalfOpenAditionalHandler = null,
    OnCircuitBreakerOpenAditionalHandler = null,
    OnCircuitBreakerCloseAditionalHandler = null,
    // Exceptions
    ExceptionHandleConfigArray = new[] {
        new Func<Exception, bool>(ex => ex.GetType() == typeof(StackExchange.Redis.RedisException)),
    }
});

var rabbitMqResiliencePolicy = dependencyInjectionContainer.Resolve<IRabbitMqResiliencePolicy>()!;
rabbitMqResiliencePolicy.Configure(() => new ResiliencePolicyConfig
{
    // Identification
    Name = appSettings.RabbitMq.ResiliencePolicy.Name,
    // Retry
    RetryMaxAttemptCount = appSettings.RabbitMq.ResiliencePolicy.RetryMaxAttemptCount,
    RetryAttemptWaitingTimeFunction = (attempt) => TimeSpan.FromMilliseconds(appSettings.RabbitMq.ResiliencePolicy.RetryAttemptWaitingTimeMilliseconds * attempt),
    OnRetryAditionalHandler = null,
    // Circuit Breaker
    CircuitBreakerWaitingTimeFunction = () => TimeSpan.FromSeconds(appSettings.RabbitMq.ResiliencePolicy.CircuitBreakerWaitingTimeSeconds),
    OnCircuitBreakerHalfOpenAditionalHandler = null,
    OnCircuitBreakerOpenAditionalHandler = null,
    OnCircuitBreakerCloseAditionalHandler = null,
    // Exceptions
    ExceptionHandleConfigArray = new[] {
        new Func<Exception, bool>(ex => ex.GetType() == typeof(RabbitMQ.Client.Exceptions.ConnectFailureException)),
    }
});

ResiliencePoliciesController.SetResiliencePolicyCollection(
    dependencyInjectionContainer.GetRegistrationCollection()
    .Where(q => typeof(IResiliencePolicy).IsAssignableFrom(q.ServiceType))
    .Select(q => (IResiliencePolicy)dependencyInjectionContainer.Resolve(q.ServiceType!)!)!
);

// Configure Metrics
var metricsManager = dependencyInjectionContainer.Resolve<IMetricsManager>()!;

metricsManager.CreateCounter<int>(name: MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Metrics.Constants.REQUESTS_COUNTER_NAME);
metricsManager.CreateCounter<int>(name: MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Metrics.Constants.EXCEPTIONS_COUNTER_NAME);

metricsManager.CreateHistogram<int>(name: MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Metrics.Constants.REQUESTS_HISTOGRAM_NAME);

McbGlobalExceptionMiddleware.SetMetricsManager(metricsManager, MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Metrics.Constants.EXCEPTIONS_COUNTER_NAME);
McbRequestCounterMetricMiddleware.SetMetricsManager(metricsManager, MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Metrics.Constants.REQUESTS_COUNTER_NAME);

StartupCheck.CompleteStartup();

#endregion

app.Run();
