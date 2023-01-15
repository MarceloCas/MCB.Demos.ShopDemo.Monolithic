using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;

public static class McbGlobalExceptionMiddleware
{
    // Fields
    private static IMetricsManager _metricsManager = null!;
    private static string _exceptionCounterName = null!;

    // Methods
    public static void SetMetricsManager(
        IMetricsManager metricsManager,
        string exceptionCounterName  
    )
    {
        _metricsManager = metricsManager;
        _exceptionCounterName = exceptionCounterName;
    }

    // Public Methods
    public static void UseMcbGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next.Invoke();
            }
            catch (Exception)
            {
                _metricsManager.IncrementCounter(_exceptionCounterName, 1);
                throw;
            }
        });
    }
}
