using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Customers.CustomerRemoved;
public class CustomerRemovedEvent
    : EventBase
{
    public CustomerDto? Customer { get; set; }
}