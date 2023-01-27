using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;

public static class McbRequestCounterMetricMiddleware
{
    // Fields
    private static ITraceManager _traceManager = null!;
    private static IMetricsManager _metricsManager = null!;
    private static string _requestCounterName = null!;

    // Methods
    public static void Init(
        ITraceManager traceManager,
        IMetricsManager metricsManager,
        string requestCounterName
    )
    {
        _traceManager = traceManager;
        _metricsManager = metricsManager;
        _requestCounterName = requestCounterName;
    }

    // Public Methods
    public static void UseMcbRequestCounterMetricMiddleware(this IApplicationBuilder app)
    {

        app.Use((context, next) =>
        {
            return _traceManager.StartActivityAsync(
                name: nameof(McbRequestCounterMetricMiddleware),
                kind: System.Diagnostics.ActivityKind.Internal,
                correlationId: Guid.Empty,
                tenantId: Guid.Empty,
                executionUser: string.Empty,
                sourcePlatform: string.Empty,
                input: (Context: context, Next: next),
                handler: (input, activity, cancellationToken) =>
                {
                    _metricsManager.IncrementCounter(
                        name: _requestCounterName,
                        delta: 1,
                        tags: new[] {
                            KeyValuePair.Create<string, object?>("path", context.Request.Path)
                        }
                    );

                    return next.Invoke();
                },
                cancellationToken: default
            );
        });
    }
}
