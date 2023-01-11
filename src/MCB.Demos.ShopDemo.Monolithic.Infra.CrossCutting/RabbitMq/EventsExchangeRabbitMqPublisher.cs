﻿using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Core.Infra.CrossCutting.RabbitMq.Connection.Interfaces;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Core.Infra.CrossCutting.RabbitMq.Publishers;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Messages.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq;
public class EventsExchangeRabbitMqPublisher
    : RabbitMqExchangePublisherBase,
    IEventsExchangeRabbitMqPublisher
{
    // Constants
    public const string MESSAGE_TYPE_PROPERTY_NAME = "mcb-message-type";
    public const string TENANT_ID_PROPERTY_NAME = "mcb-tenant-id";
    public const string SOURCE_PLATFORM_PROPERTY_NAME = "mcb-source-platform";
    public const string EXECUTION_USER_PROPERTY_NAME = "mcb-execution-user-platform";
    public const string CORRELATION_ID_PROPERTY_NAME = "mcb-correlation-id";

    // Fields
    private readonly IProtobufSerializer _protobufSerializer;

    // Constructors
    public EventsExchangeRabbitMqPublisher(
        IRabbitMqConnection connection, 
        RabbitMqExchangeConfig exchangeConfig,
        IProtobufSerializer protobufSerializer
    ) : base(connection, exchangeConfig)
    {
        _protobufSerializer = protobufSerializer;
    }

    // Public Methods
    protected override IDictionary<string, object>? GetBasicProperties(object subject, Type subjectBaseType)
    {
        var dictionary = new Dictionary<string, object> {
            { MESSAGE_TYPE_PROPERTY_NAME, subjectBaseType.FullName! }
        };

        if(subject is EventBase eventBase)
        {
            dictionary.Add(key: TENANT_ID_PROPERTY_NAME, value: eventBase.TenantId.ToString());
            dictionary.Add(key: SOURCE_PLATFORM_PROPERTY_NAME, value: eventBase.SourcePlatform);
            dictionary.Add(key: EXECUTION_USER_PROPERTY_NAME, value: eventBase.ExecutionUser);
            dictionary.Add(key: CORRELATION_ID_PROPERTY_NAME, value: eventBase.CorrelationId.ToString());
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
}
