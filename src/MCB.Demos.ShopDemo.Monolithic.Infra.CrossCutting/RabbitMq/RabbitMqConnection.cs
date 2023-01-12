using MCB.Core.Infra.CrossCutting.RabbitMq.Connection;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;
using RabbitMQ.Client;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq;
public class RabbitMqConnection
    : RabbitMqConnectionBase
{
    // Fields
    private readonly IRabbitMqResiliencePolicy _rabbitMqResiliencePolicy;

    // Constructors
    public RabbitMqConnection(
        RabbitMqConnectionConfig connectionConfig,
        IRabbitMqResiliencePolicy rabbitMqResiliencePolicy 
    ) : base(connectionConfig)
    {
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
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.PublishQueue(input.QueueConfig, input.Properties, input.Message);
                    return Task.CompletedTask;
                },
                input: (QueueConfig: queueConfig, Properties: properties, Message: message),
                cancellationToken: default
            );
        }).Result;
    }
    public override void PublishExchange(RabbitMqExchangeConfig exchangeConfig, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> message)
    {
        _ = Task.Run(() =>
        {
            return _rabbitMqResiliencePolicy.ExecuteAsync(
                handler: (input, cancellationToken) =>
                {
                    base.PublishExchange(exchangeConfig, routingKey, properties, message);
                    return Task.CompletedTask;
                },
                input: (ExchangeConfig: exchangeConfig, RoutingKey: routingKey, Properties: properties, Message: message),
                cancellationToken: default
            );
        }).Result;
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
