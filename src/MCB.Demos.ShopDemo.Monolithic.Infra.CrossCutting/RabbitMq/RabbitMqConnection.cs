using MCB.Core.Infra.CrossCutting.RabbitMq.Connection;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq;
public class RabbitMqConnection
    : RabbitMqConnectionBase
{
    public RabbitMqConnection(
        RabbitMqConnectionConfig connectionConfig
    ) : base(connectionConfig)
    {
    }
}
