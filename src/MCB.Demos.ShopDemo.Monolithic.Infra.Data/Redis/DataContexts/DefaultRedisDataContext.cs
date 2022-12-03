using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts;

public class DefaultRedisDataContext
    : RedisDataContextBase,
    IDefaultRedisDataContext
{
    public DefaultRedisDataContext(
        RedisOptions redisOptions
    ) : base(redisOptions)
    {
    }
}