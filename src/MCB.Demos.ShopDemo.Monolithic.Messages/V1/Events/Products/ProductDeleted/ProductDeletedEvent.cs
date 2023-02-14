using MCB.Demos.ShopDemo.Monolithic.Messages.Base;
using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Events.Products.ProductDeleted;
public class ProductDeletedEvent
    : EventBase
{
    public ProductDto? Product { get; set; }
}
