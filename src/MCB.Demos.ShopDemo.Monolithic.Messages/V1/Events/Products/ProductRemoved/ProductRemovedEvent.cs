using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductRemoved;
public class ProductRemovedEvent
    : EventBase
{
    public ProductDto? Product { get; set; }
}
