using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Core.Infra.CrossCutting.Observability.OpenTelemetry;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;

public static class McbGlobalExceptionMiddleware
{
    // Fields
    private static ITraceManager _traceManager = null!;
    private static IMetricsManager _metricsManager = null!;
    private static string _exceptionCounterName = null!;

    // Methods
    public static void Init(
        ITraceManager traceManager,
        IMetricsManager metricsManager,
        string exceptionCounterName  
    )
    {
        _traceManager = traceManager;
        _metricsManager = metricsManager;
        _exceptionCounterName = exceptionCounterName;
    }

    // Public Methods
    public static void UseMcbGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        app.Use((context, next) =>
        {
            return _traceManager.StartActivityAsync(
                name: nameof(McbGlobalExceptionMiddleware),
                kind: System.Diagnostics.ActivityKind.Internal,
                correlationId: Guid.Empty,
                tenantId: Guid.Empty,
                executionUser: string.Empty,
                sourcePlatform: string.Empty,
                input: (Context: context, Next: next),
                handler: (input, activity, cancellationToken) =>
                {
                    try
                    {
                        return next.Invoke();
                    }
                    catch (Exception)
                    {
                        _metricsManager.IncrementCounter(_exceptionCounterName, 1);
                        throw;
                    }
                },
                cancellationToken: default
            );
        });
    }
}
