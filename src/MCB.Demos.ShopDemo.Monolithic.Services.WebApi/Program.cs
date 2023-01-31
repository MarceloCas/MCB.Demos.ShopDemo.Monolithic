using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;
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
using OpenTelemetry.Logs;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Logging;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

#region Config
var appSettings = builder.Configuration.Get<AppSettings>();

if (appSettings == null)
    throw new InvalidOperationException("AppSettings cannot be null");

builder.Services.AddSingleton(appSettings);
#endregion

#region Configure Service

// McbDependencyInjection 
builder.Services.AddMcbDependencyInjection(dependencyInjectionContainer =>
{
    dependencyInjectionContainer.RegisterSingleton<IStartupService, StartupService>();

    MCB.Demos.ShopDemo.Monolithic.Services.WebApi.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(
        applicationName: appSettings.ApplicationName,
        applicationVersion: appSettings.ApplicationVersion,
        dependencyInjectionContainer,
        adapterMapAction: typeAdapterConfig => AdapterConfig.Configure(dependencyInjectionContainer),
        appSettings!
    );
});

// OpenTelemetry Tracing and Metrics
var batchExportProcessorOptions = new BatchExportProcessorOptions<System.Diagnostics.Activity>
{
    MaxQueueSize = 2048,
    ExporterTimeoutMilliseconds = 30_000,
    MaxExportBatchSize = 512,
    ScheduledDelayMilliseconds = 5000
};

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
        .AddRedisInstrumentation(configure: options => { })
        .AddNpgsql(options => { })
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(appSettings.OpenTelemetry.GrpcCollectorReceiverUrl);
            options.BatchExportProcessorOptions = batchExportProcessorOptions;
        })
    )
    .WithMetrics(builder => builder
        .AddMeter(appSettings.ApplicationName)
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(appSettings.OpenTelemetry.GrpcCollectorReceiverUrl);
            options.BatchExportProcessorOptions = batchExportProcessorOptions;
        })
    )
.StartWithHost();

// OpenTelemetry Logging
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
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(appSettings.OpenTelemetry.GrpcCollectorReceiverUrl);
            options.BatchExportProcessorOptions = batchExportProcessorOptions;
        })
        .AddProcessor(new OpenTelemetryLogGrayLogProcessor());

    if (appSettings.OpenTelemetry.EnableConsoleExporter)
        options.AddConsoleExporter();

});
builder.Logging.AddFilter<OpenTelemetryLoggerProvider>("*", Enum.Parse<LogLevel>(appSettings.Logging.LogLevel.Default));

// Entity Framework
builder.Services.AddDbContextPool<PostgreSqlEntityFrameworkDataContext>(
    options => options.UseNpgsql(
        appSettings!.PostgreSql.ConnectionString
    )
);
builder.Services.AddScoped<IEntityFrameworkDataContext>(serviceCollection =>
{
    var appSettings = serviceCollection.GetService<AppSettings>()!;

    return new PostgreSqlEntityFrameworkDataContext(
        serviceCollection.GetService<ITraceManager>()!,
        appSettings.PostgreSql.ConnectionString,
        serviceCollection.GetService<IDependencyInjectionContainer>()!,
        serviceCollection.GetService<IPostgreSqlResiliencePolicy>()!
    );
});

// ASP.NET
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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

// HealthCheck
builder.Services.AddHealthChecks()
    .AddCheck<StartupCheck>("/health/startup", tags: new[] { "startup" })
    .AddCheck<ReadinessCheck>("/health/readiness", tags: new[] { "readiness" })
    .AddCheck<LivenessCheck>("/health/liveness", tags: new[] { "liveness" });
#endregion

var app = builder.Build();
var logger = app.Services.GetService<ILogger<Program>>()!;

#region Configure Pipeline
app.UseMcbGlobalExceptionMiddleware();
app.UseMcbRequestCounterMetricMiddleware();
app.UseMcbDependencyInjection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseAuthorization();
app.MapControllers();

// HealthCheck
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
var dependencyInjectionContainer = app.Services.GetService<IDependencyInjectionContainer>()!;
var startupService = dependencyInjectionContainer.Resolve<IStartupService>()!;
var tryStartupApplicationResult = await startupService.TryStartupApplicationAsync(cancellationToken: default);

if (!tryStartupApplicationResult.Success)
{
    logger.LogError("Fail on startup");

    if (tryStartupApplicationResult.Messages != null)
        foreach (var message in tryStartupApplicationResult.Messages)
            logger.LogError(message: "Startup error message: {message}", args: message!);
}
#endregion

app.Run();
