using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base;

public abstract class QueryBase
{
    // Properties
    protected ITraceManager TraceManager { get; }

    // Constructors
    protected QueryBase(ITraceManager traceManager)
    {
        TraceManager = traceManager;
    }
}
public abstract class QueryBase<TQueryInput, TQueryResult>
    : QueryBase,
    IQuery<TQueryInput, TQueryResult>
    where TQueryInput : QueryInputBase
{
    // Constructors
    protected QueryBase(ITraceManager traceManager) 
        : base(traceManager)
    {
    }

    // Abstract Methods
    public abstract Task<TQueryResult> ExecuteAsync(TQueryInput input, CancellationToken cancellationToken);
}
