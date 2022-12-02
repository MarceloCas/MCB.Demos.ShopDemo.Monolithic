using MCB.Core.Domain.Entities.Abstractions;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Abstractions;

public interface IService<TAggregationRoot>
    where TAggregationRoot : IAggregationRoot
{
}