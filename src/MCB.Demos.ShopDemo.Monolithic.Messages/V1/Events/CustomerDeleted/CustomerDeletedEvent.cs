using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.CustomerDeleted;
public class CustomerDeletedEvent
    : EventBase
{
    public CustomerDto? Customer { get; set; }
}