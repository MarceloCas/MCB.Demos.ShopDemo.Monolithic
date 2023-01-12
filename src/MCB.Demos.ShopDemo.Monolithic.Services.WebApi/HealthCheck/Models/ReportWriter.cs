using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.HealthCheck.Models;

public static class ReportWriter
{
    // Fields
    private static readonly JsonSerializerOptions _jsonSerializeOptions = new()
    {
        PropertyNameCaseInsensitive = false
    };

    // Methods
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

}
