﻿using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.CustomerHasBeenRegistered;

public record CustomerHasBeenRegisteredEvent
    : EventBase
{
    public CustomerDto? Customer { get; set; }
}