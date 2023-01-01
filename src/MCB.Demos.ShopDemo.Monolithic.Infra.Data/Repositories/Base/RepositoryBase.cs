using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories.Base;

public abstract class RepositoryBase
{
    // Properties
    protected ITraceManager TraceManager { get; }
    protected IAdapter Adapter { get; }

    // Conscctuctors
    protected RepositoryBase(
        ITraceManager traceManager,
        IAdapter adapter
    )
    {
        TraceManager = traceManager;
        Adapter = adapter;
    }
}