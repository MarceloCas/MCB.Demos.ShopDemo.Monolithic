using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Config
var appSettings = builder.Configuration.Get<AppSettings>();
#endregion

#region Configure Service
builder.Services.AddMcbDependencyInjection(dependencyInjectionContainer =>
    MCB.Demos.ShopDemo.Monolithic.Services.WebApi.DependencyInjection.Bootstrapper.ConfigureDependencyInjection(
        dependencyInjectionContainer,
        adapterMapAction: typeAdapterConfig => AdapterConfig.Configure(typeAdapterConfig),
        appSettings
    )
);

builder.Services.AddControllers();
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
