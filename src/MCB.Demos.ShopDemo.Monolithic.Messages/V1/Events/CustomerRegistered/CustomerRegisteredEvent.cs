using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.CustomerRegistered;

public class CustomerRegisteredEvent
    : EventBase
{
    public CustomerDto? Customer { get; set; }
}