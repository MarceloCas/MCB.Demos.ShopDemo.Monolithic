using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Grpc.Core.Metadata;

var builder = WebApplication.CreateBuilder(args);

#region Config
var appSettings = builder.Configuration.Get<AppSettings>();

if (appSettings == null)
    throw new InvalidOperationException("AppSettings cannot be null");

Console.WriteLine("AppSettings:");
Console.Write(JsonSerializer.Serialize(appSettings, new JsonSerializerOptions() { WriteIndented = true }));

Console.WriteLine("\nEnvironment variables:");
foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
    Console.WriteLine("{0} = {1}", entry.Key, entry.Value);


Console.WriteLine("\nPostgres:");
Console.WriteLine(appSettings.PostgreSql.ConnectionString.Length.ToString());
Console.WriteLine(appSettings.PostgreSql.ConnectionString.ToString());

#endregion

#region Configure Service
builder.Services.AddMcbDependencyInjection(dependencyInjectionContainer =>
    MCB.Demos.ShopDemo.Monolithic.Services.WebApi.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(
        applicationName: appSettings.ApplicationName,
        dependencyInjectionContainer,
        adapterMapAction: typeAdapterConfig => AdapterConfig.Configure(dependencyInjectionContainer),
        appSettings!
    )
);

builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddOtlpExporter(opt =>
        {
            opt.Protocol = OtlpExportProtocol.Grpc;
        })
        .AddSource(appSettings.ApplicationName)
        .SetResourceBuilder(
            ResourceBuilder
                .CreateDefault()
                .AddService(serviceName: appSettings.ApplicationName, serviceVersion: appSettings.ApplicationVersion)
        )
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();
});

builder.Services.AddDbContextPool<DefaultEntityFrameworkDataContext>(
    options => options.UseNpgsql(
        appSettings!.PostgreSql.ConnectionString
    )
);
builder.Services.AddScoped<IEntityFrameworkDataContext>(serviceCollection => {
    var config = serviceCollection.GetService<IConfiguration>()!;

    return new DefaultEntityFrameworkDataContext(
        serviceCollection.GetService<ITraceManager>()!,
        config["PostgreSql:ConnectionString"]!
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
builder.Services.AddHealthChecks().AddCheck<DefaultHealthCheck>("Default");
#endregion

#region Configure Pipeline
var app = builder.Build();

app.UseMcbDependencyInjection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
#endregion

app.Run();
