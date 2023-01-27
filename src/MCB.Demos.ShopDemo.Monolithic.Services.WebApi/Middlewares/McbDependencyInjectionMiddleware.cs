using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;

public static class McbDependencyInjectionMiddleware
{
    // Fields
    private static DependencyInjectionContainer? _dependencyInjection;
    private static ITraceManager _traceManager = null!;

    public static void Init(
        DependencyInjectionContainer? dependencyInjection,
        ITraceManager traceManager    
    )
    {
        _dependencyInjection = dependencyInjection;
        _traceManager = traceManager;
    }

    /// <summary>
    /// Add before MapControllers and MapGrpcService
    /// </summary>
    /// <param name="app"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void UseMcbDependencyInjection(this IApplicationBuilder app)
    {
        app.Use((context, next) =>
        {
            return _traceManager.StartActivityAsync(
                name: nameof(McbDependencyInjectionMiddleware),
                kind: System.Diagnostics.ActivityKind.Internal,
                correlationId: Guid.Empty,
                tenantId: Guid.Empty,
                executionUser: string.Empty,
                sourcePlatform: string.Empty,
                input: (Context: context, Next: next),
                handler: (input, activity, cancellationToken) =>
                {
                    if(_dependencyInjection is null)
                        throw new InvalidOperationException($"{nameof(DependencyInjectionContainer)} cannot be null");

                    _dependencyInjection.Build(context.RequestServices);

                    return input.Next.Invoke();
                },
                cancellationToken: default
            );
        });
    }
}