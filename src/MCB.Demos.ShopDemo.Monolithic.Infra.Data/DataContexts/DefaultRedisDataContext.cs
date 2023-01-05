using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts;

public class DefaultRedisDataContext
    : RedisDataContextBase,
    IRedisDataContext
{
    public DefaultRedisDataContext(
        RedisOptions redisOptions
    ) : base(redisOptions)
    {
    }
}