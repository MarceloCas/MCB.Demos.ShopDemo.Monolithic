namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base.Interfaces;
public interface IQuery
{

}
public interface IQuery<TQueryInput, TQueryResult>
    : IQuery
    where TQueryInput : QueryInputBase
{
    Task<TQueryResult> ExecuteAsync(TQueryInput input, CancellationToken cancellationToken);
}
