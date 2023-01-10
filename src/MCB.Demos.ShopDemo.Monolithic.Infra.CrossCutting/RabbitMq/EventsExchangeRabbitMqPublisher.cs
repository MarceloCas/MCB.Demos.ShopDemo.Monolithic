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

    // Constructors
    public EventsExchangeRabbitMqPublisher(
        IRabbitMqConnection connection, 
        RabbitMqExchangeConfig exchangeConfig
    ) : base(connection, exchangeConfig)
    {
    }

    // Public Methods
    protected override IDictionary<string, object>? GetBasicProperties(object subject, Type subjectBaseType)
    {
        var dictionary = new Dictionary<string, object> {
            { MESSAGE_TYPE_PROPERTY_NAME, subjectBaseType.FullName! }
        };

        if(subject is EventBase eventBase)
        {
            dictionary.Add(key: TENANT_ID_PROPERTY_NAME, value: eventBase.TenantId);
            dictionary.Add(key: SOURCE_PLATFORM_PROPERTY_NAME, value: eventBase.SourcePlatform);
        }

        return dictionary;
    }

    protected override (Guid TenantId, Guid CorrelationId, string ExecutionUser, string SourcePlatform) GetRabbitMqMessageEnvelopInfo(object subject, Type subjectBaseType)
    {
        throw new NotImplementedException();
    }

    protected override string GetRoutingKey(object subject, Type subjectBaseType)
    {
        throw new NotImplementedException();
    }

    protected override ReadOnlyMemory<byte>? SerializeMessage(object subject, Type subjectBaseType)
    {
        throw new NotImplementedException();
    }

    protected override ReadOnlyMemory<byte>? SerializeRabbitMqEnvelopMessage(RabbitMqMessageEnvelop rabbitMqMessageEnvelop)
    {
        throw new NotImplementedException();
    }
}
