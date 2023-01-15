using MCB.Core.Infra.CrossCutting.Observability.Abstractions;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;

public static class McbRequestCounterMetricMiddleware
{
    // Fields
    private static IMetricsManager _metricsManager = null!;
    private static string _requestCounterName = null!;

    // Methods
    public static void SetMetricsManager(
        IMetricsManager metricsManager,
        string requestCounterName
    )
    {
        _metricsManager = metricsManager;
        _requestCounterName = requestCounterName;
    }

    // Public Methods
    public static void UseMcbRequestCounterMetricMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                _metricsManager.IncrementCounter(
                    name: _requestCounterName, 
                    delta: 1,
                    tags: new[] { 
                        KeyValuePair.Create<string, object?>("path", context.Request.Path)
                    }
                );

                await next.Invoke();
            }
            catch (Exception)
            {
                
                throw;
            }
        });
    }
}
