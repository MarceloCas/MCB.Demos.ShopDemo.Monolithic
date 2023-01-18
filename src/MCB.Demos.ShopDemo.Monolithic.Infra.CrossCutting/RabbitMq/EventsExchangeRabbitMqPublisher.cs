using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Core.Infra.CrossCutting.RabbitMq.Connection.Interfaces;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Core.Infra.CrossCutting.RabbitMq.Publishers;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Messages.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq;
public class EventsExchangeRabbitMqPublisher
    : RabbitMqExchangePublisherBase,
    IEventsExchangeRabbitMqPublisher
{
    public const string PUBLISH_ASYNC_TRACE_NAME = $"{nameof(EventsExchangeRabbitMqPublisher)}.{nameof(PublishAsync)}";

    // Fields
    private readonly ITraceManager _traceManager;
    private readonly IProtobufSerializer _protobufSerializer;
    private readonly IRabbitMqResiliencePolicy _rabbitMqResiliencePolicy;

    // Constructors
    public EventsExchangeRabbitMqPublisher(
        IRabbitMqConnection connection, 
        RabbitMqExchangeConfig exchangeConfig,
        ITraceManager traceManager,
        IRabbitMqResiliencePolicy rabbitMqResiliencePolicy,
        IProtobufSerializer protobufSerializer
    ) : base(connection, exchangeConfig)
    {
        _protobufSerializer = protobufSerializer;
        _rabbitMqResiliencePolicy = rabbitMqResiliencePolicy;
        _traceManager = traceManager;
    }

    // Public Methods
    protected override IDictionary<string, object>? GetBasicProperties(object subject, Type subjectBaseType)
    {
        var dictionary = new Dictionary<string, object> {
            { RabbitMqConnection.MESSAGE_TYPE_PROPERTY_NAME, subjectBaseType.FullName! }
        };

        if(subject is EventBase eventBase)
        {
            dictionary.Add(key: RabbitMqConnection.TENANT_ID_PROPERTY_NAME, value: eventBase.TenantId.ToString());
            dictionary.Add(key: RabbitMqConnection.SOURCE_PLATFORM_PROPERTY_NAME, value: eventBase.SourcePlatform);
            dictionary.Add(key: RabbitMqConnection.EXECUTION_USER_PROPERTY_NAME, value: eventBase.ExecutionUser);
            dictionary.Add(key: RabbitMqConnection.CORRELATION_ID_PROPERTY_NAME, value: eventBase.CorrelationId.ToString());
        }

        return dictionary;
    }
    protected override (Guid TenantId, Guid CorrelationId, string ExecutionUser, string SourcePlatform) GetRabbitMqMessageEnvelopInfo(object subject, Type subjectBaseType)
    {
        return subject is EventBase eventBase
            ? (eventBase.TenantId, eventBase.CorrelationId, eventBase.ExecutionUser, eventBase.SourcePlatform)
            : ((Guid TenantId, Guid CorrelationId, string ExecutionUser, string SourcePlatform))default;
    }

    protected override string GetRoutingKey(object subject, Type subjectBaseType)
    {
        return string.Empty;
    }

    protected override ReadOnlyMemory<byte>? SerializeMessage(object subject, Type subjectBaseType)
    {
        return _protobufSerializer.SerializeToProtobuf(subject);
    }
    protected override ReadOnlyMemory<byte>? SerializeRabbitMqEnvelopMessage(RabbitMqMessageEnvelop rabbitMqMessageEnvelop)
    {
        return _protobufSerializer.SerializeToProtobuf(rabbitMqMessageEnvelop);
    }

    public override Task PublishAsync<TSubject>(TSubject subject, Type subjectBaseType, CancellationToken cancellationToken)
    {
        var eventBase = subject as EventBase;

        return _rabbitMqResiliencePolicy.ExecuteAsync(
            handler: (input, cancellationToken) =>
            {
                return input.TraceManager.StartActivityAsync(
                    name: PUBLISH_ASYNC_TRACE_NAME,
                    kind: System.Diagnostics.ActivityKind.Internal,
                    correlationId: input.EventBase?.CorrelationId ?? Guid.Empty,
                    tenantId: input.EventBase?.TenantId ?? Guid.Empty,
                    executionUser: input.EventBase?.ExecutionUser,
                    sourcePlatform: input.EventBase?.SourcePlatform,
                    input: (input.Subject, input.SubjectBaseType),
                    handler: (input, activity, cancellationToken) =>
                    {
                        return base.PublishAsync(input.Subject, input.SubjectBaseType, cancellationToken);
                    },
                    cancellationToken
                );
            },
            input: (EventBase: eventBase, Subject: subject, SubjectBaseType: subjectBaseType, TraceManager: _traceManager),
            cancellationToken
        );
    }
}
