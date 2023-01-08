using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;

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