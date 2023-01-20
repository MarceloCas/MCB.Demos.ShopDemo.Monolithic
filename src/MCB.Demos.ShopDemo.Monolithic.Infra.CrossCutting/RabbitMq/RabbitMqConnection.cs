using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Core.Infra.CrossCutting.RabbitMq.Connection;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;
using RabbitMQ.Client;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq;
public class RabbitMqConnection
    : RabbitMqConnectionBase
{

    // Constants
    public const string MESSAGE_TYPE_PROPERTY_NAME = "mcb-message-type";
    public const string QUEUE_NAME_PROPERTY_NAME = "mcb-queue-name";
    public const string EXCHANGE_NAME_PROPERTY_NAME = "mcb-exchange-name";
    public const string TENANT_ID_PROPERTY_NAME = "mcb-tenant-id";
    public const string SOURCE_PLATFORM_PROPERTY_NAME = "mcb-source-platform";
    public const string EXECUTION_USER_PROPERTY_NAME = "mcb-execution-user-platform";
    public const string CORRELATION_ID_PROPERTY_NAME = "mcb-correlation-id";

    // Fields
    private readonly ITraceManager _traceManager;
    private readonly IRabbitMqResiliencePolicy _rabbitMqResiliencePolicy;

    // Constructors
    public RabbitMqConnection(
        RabbitMqConnectionConfig connectionConfig,
        ITraceManager traceManager,
        IRabbitMqResiliencePolicy rabbitMqResiliencePolicy
    ) : base(connectionConfig)
    {
        _traceManager = traceManager;
        _rabbitMqResiliencePolicy = rabbitMqResiliencePolicy;
    }

    // Public Methods
    public override void OpenConnection(bool forceReopen = false)
    {
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.OpenConnection(input);

                    return Task.CompletedTask;
                },
                input: forceReopen,
                cancellationToken: default
            );
        }).Result;
    }
    public override QueueDeclareOk? QueueDeclare(RabbitMqQueueConfig queueConfig)
    {
        return Task.Run(async () =>
        {
            var result = await _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    return Task.FromResult(base.QueueDeclare(input!));
                },
                input: queueConfig,
                cancellationToken: default
            );

            return result.Output;
        }).Result;
    }
    public override void ExchangeDeclare(RabbitMqExchangeConfig exchangeConfig)
    {
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.ExchangeDeclare(input!);
                    return Task.CompletedTask;
                },
                input: exchangeConfig,
                cancellationToken: default
            );
        }).Result;
    }
    public override bool CheckIfQueueExists(string queueName)
    {
        return Task.Run(async () =>
        {
            var result = await _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    return Task.FromResult(base.CheckIfQueueExists(input!));
                },
                input: queueName,
                cancellationToken: default
            );

            return result.Output;
        }).Result;
    }
    public override bool CheckIfExchangeExists(string exchangeName)
    {
        return Task.Run(async () =>
        {
            var result = await _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    return Task.FromResult(base.CheckIfExchangeExists(input!));
                },
                input: exchangeName,
                cancellationToken: default
            );

            return result.Output;
        }).Result;
    }

    public override void PublishQueue(RabbitMqQueueConfig queueConfig, IBasicProperties properties, ReadOnlyMemory<byte> message)
    {
        var messageType = string.Empty;
        if (properties.Headers.TryGetValue(MESSAGE_TYPE_PROPERTY_NAME, out object? messageTypeObject))
            messageType = messageTypeObject.ToString()!;

        var correlationId = Guid.Empty;
        if (properties.Headers.TryGetValue(CORRELATION_ID_PROPERTY_NAME, out object? correlationIdObject))
            correlationId = Guid.Parse(correlationIdObject.ToString()!);

        var tenantId = Guid.Empty;
        if (properties.Headers.TryGetValue(TENANT_ID_PROPERTY_NAME, out object? tenantIdObject))
            tenantId = Guid.Parse(tenantIdObject.ToString()!);

        var executionUser = string.Empty;
        if (properties.Headers.TryGetValue(EXECUTION_USER_PROPERTY_NAME, out object? executionUserObject))
            executionUser = executionUserObject!.ToString();

        var sourcePlatform = string.Empty;
        if (properties.Headers.TryGetValue(SOURCE_PLATFORM_PROPERTY_NAME, out object? sourcePlatformObject))
            sourcePlatform = sourcePlatformObject!.ToString();

        _traceManager.StartActivity(
            name: $"{nameof(PublishQueue)} [{queueConfig.QueueName}]",
            kind: System.Diagnostics.ActivityKind.Producer,
            correlationId,
            tenantId,
            executionUser,
            sourcePlatform,
            input: (QueueConfig: queueConfig, Properties: properties, Message: message, MessageType: messageType, RabbitMqResiliencePolicy: _rabbitMqResiliencePolicy),
            handler: (input, activity) =>
            {
                activity.AddTag(MESSAGE_TYPE_PROPERTY_NAME, input.MessageType);
                activity.AddTag(QUEUE_NAME_PROPERTY_NAME, input.QueueConfig.QueueName);

                _ = Task.Run(() =>
                {
                    return input.RabbitMqResiliencePolicy.ExecuteAsync(
                        handler: (input, cancellationToken) =>
                        {
                            base.PublishQueue(input.QueueConfig, input.Properties, input.Message);
                            return Task.CompletedTask;
                        },
                        input,
                        cancellationToken: default
                    );
                }).Result;
            }
        );
    }
    public override void PublishExchange(RabbitMqExchangeConfig exchangeConfig, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> message)
    {
        var correlationId = Guid.Empty;
        if (properties.Headers.TryGetValue(CORRELATION_ID_PROPERTY_NAME, out object? correlationIdObject))
            correlationId = Guid.Parse(correlationIdObject.ToString()!);

        var tenantId = Guid.Empty;
        if (properties.Headers.TryGetValue(TENANT_ID_PROPERTY_NAME, out object? tenantIdObject))
            tenantId = Guid.Parse(tenantIdObject.ToString()!);

        var executionUser = string.Empty;
        if (properties.Headers.TryGetValue(EXECUTION_USER_PROPERTY_NAME, out object? executionUserObject))
            executionUser = executionUserObject!.ToString();

        var sourcePlatform = string.Empty;
        if (properties.Headers.TryGetValue(SOURCE_PLATFORM_PROPERTY_NAME, out object? sourcePlatformObject))
            sourcePlatform = sourcePlatformObject!.ToString();

        _traceManager.StartActivity(
            name: $"{nameof(PublishExchange)} [{exchangeConfig.ExchangeName}]",
            kind: System.Diagnostics.ActivityKind.Producer,
            correlationId,
            tenantId,
            executionUser,
            sourcePlatform,
            input: (ExchangeConfig: exchangeConfig, RoutingKey: routingKey, Properties: properties, Message: message, RabbitMqResiliencePolicy: _rabbitMqResiliencePolicy),
            handler: (input, activity) =>
            {
                activity.AddTag(EXCHANGE_NAME_PROPERTY_NAME, input.ExchangeConfig.ExchangeName);

                _ = Task.Run(() =>
                {
                    return input.RabbitMqResiliencePolicy.ExecuteAsync(
                        handler: (input, cancellationToken) =>
                        {
                            base.PublishExchange(input.ExchangeConfig, input.RoutingKey, input.Properties, input.Message);
                            return Task.CompletedTask;
                        },
                        input,
                        cancellationToken: default
                    );
                }).Result;
            }
        );
        
    }

    public override (uint messageCount, uint consumerCount)? GetQueueCounters(string queueName)
    {
        return Task.Run(async () =>
        {
            var result = await _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    return Task.FromResult(base.GetQueueCounters(input!));
                },
                input: queueName,
                cancellationToken: default
            );

            return result.Output;
        }).Result;
    }
    public override bool DeleteQueue(string queueName, bool ifUnused = false, bool ifEmpty = false)
    {
        return Task.Run(async () =>
        {
            var result = await _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    return Task.FromResult(base.DeleteQueue(input.QueueName, input.IfUnused, input.IfEmpty));
                },
                input: (QueueName: queueName, IfUnused: ifUnused, IfEmpty: ifEmpty),
                cancellationToken: default
            );

            return result.Output;
        }).Result;
    }
    public override void PurgeQueue(string queueName)
    {
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.PurgeQueue(input!);
                    return Task.CompletedTask;
                },
                input: queueName,
                cancellationToken: default
            );
        }).Result;
    }
    public override void DisconectConsumer(string consumerTag)
    {
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.DisconectConsumer(input!);
                    return Task.CompletedTask;
                },
                input: consumerTag,
                cancellationToken: default
            );
        }).Result;
    }
    public override void Ack(ulong deliveryTag, bool multiple)
    {
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.Ack(input.DeliveryTag, input.Multiple);
                    return Task.CompletedTask;
                },
                input: (DeliveryTag: deliveryTag, Multiple: multiple),
                cancellationToken: default
            );
        }).Result;
    }
    public override void Nack(ulong deliveryTag, bool multiple, bool requeue)
    {
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.Nack(input.DeliveryTag, input.Multiple, input.Requeue);
                    return Task.CompletedTask;
                },
                input: (DeliveryTag: deliveryTag, Multiple: multiple, Requeue: requeue),
                cancellationToken: default
            );
        }).Result;
    }
}
