using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;

public static class McbDependencyInjectionMiddleware
{
    /// <summary>
    /// Add before MapControllers and MapGrpcService
    /// </summary>
    /// <param name="app"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void UseMcbDependencyInjection(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var dependencyInjectionContainerAbstraction = app.ApplicationServices.GetService<IDependencyInjectionContainer>();

            if (dependencyInjectionContainerAbstraction is null)
                throw new InvalidOperationException($"Cannot resolve {typeof(IDependencyInjectionContainer).Name}");

            var dependencyInjectionContainer = (DependencyInjectionContainer)dependencyInjectionContainerAbstraction;

            dependencyInjectionContainer.Build(context.RequestServices);

            await next.Invoke();
        });
    }
}