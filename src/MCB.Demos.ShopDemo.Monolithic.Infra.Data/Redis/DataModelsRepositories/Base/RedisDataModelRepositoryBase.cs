using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Base.Interfaces;
using StackExchange.Redis;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Base;

public abstract class RedisDataModelRepositoryBase
{

    // Properties
    protected ITraceManager TraceManager { get; }
    protected IRedisDataContext RedisDataContext { get; }
    protected IJsonSerializer JsonSerializer { get; }

    protected RedisDataModelRepositoryBase(
        ITraceManager traceManager,
        IRedisDataContext redisDataContext,
        IJsonSerializer jsonSerializer
    )
    {
        TraceManager = traceManager;
        RedisDataContext = redisDataContext;
        JsonSerializer = jsonSerializer;
    }
}
public abstract class RedisDataModelRepositoryBase<TDataModel>
    : RedisDataModelRepositoryBase,
    IRedisDataModelRepository<TDataModel>
    where TDataModel : DataModelBase
{
    // Constants
    public static readonly string ADD_OR_UPDATE_TRACE_NAME = $"{nameof(RedisDataModelRepositoryBase)}.{typeof(TDataModel).Name}.{nameof(AddOrUpdateAsync)}";
    public static readonly string REMOVE_TRACE_NAME = $"{nameof(RedisDataModelRepositoryBase)}.{typeof(TDataModel).Name}.{nameof(GetAsync)}";
    public static readonly string GET_TRACE_NAME = $"{nameof(RedisDataModelRepositoryBase)}.{typeof(TDataModel).Name}.{nameof(GetAsync)}";

    // Constructors
    protected RedisDataModelRepositoryBase(
        ITraceManager traceManager,
        IRedisDataContext redisDataContext,
        IJsonSerializer jsonSerializer
    ) : base(traceManager, redisDataContext, jsonSerializer)
    {

    }

    // Public Methods
    public abstract string? GetKey(TDataModel? dataModel);
    public virtual Task<bool> AddOrUpdateAsync(TDataModel dataModel, TimeSpan? expiry, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: ADD_OR_UPDATE_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: dataModel.TenantId,
            executionUser: dataModel.LastUpdatedBy ?? dataModel.CreatedBy,
            sourcePlatform: dataModel.LastSourcePlatform,
            input: (DataModel: dataModel, Expiry: expiry, RedisDataContext, JsonSerializer),
            handler: (input, activity, cancellationToken) =>
            {
                var key = GetKey(input.DataModel);

                if (key is null)
                    return Task.FromResult(false);

                return input.RedisDataContext.StringSetAsync(
                    key,
                    value: input.JsonSerializer.SerializeToJson(input.DataModel),
                    input.Expiry
                );
            },
            cancellationToken: default
        );
    }
    public virtual Task<bool> RemoveAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: REMOVE_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: Guid.Empty,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: (DataModel: dataModel, RedisDataContext),
            handler: (input, activity, cancellationToken) =>
            {
                var key = GetKey(input.DataModel);

                if (key is null)
                    return Task.FromResult(false);

                return input.RedisDataContext.RemoveAsync(
                    key
                );
            },
            cancellationToken: default
        );
        
    }
    public virtual Task<TDataModel?> GetAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        return TraceManager.StartActivityAsync(
            name: GET_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: Guid.Empty,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: (Key: key, RedisDataContext, JsonSerializer, CommandFlags: commandFlags),
            handler: async (input, activity, cancellationToken) =>
            {
                var value = await input.RedisDataContext.StringGetAsync(input.Key!, input.CommandFlags);
                return value.IsNull 
                    ? null 
                    : input.JsonSerializer.DeserializeFromJson<TDataModel>(value!);
            },
            cancellationToken: default
        );
    }
}
