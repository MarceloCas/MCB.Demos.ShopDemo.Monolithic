using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
public record ImportOrderInput
    : InputBase
{
    // Properties
    public string Code { get; }
    public DateTime Date { get; }
    public Customers.Customer Customer { get; }
    public IEnumerable<OrderItem> OrderItemCollection { get; }

    // Constructors
    public ImportOrderInput(
        Guid correlationId,
        Guid tenantId,
        string code,
        DateTime date,
        Customers.Customer customer,
        string executionUser,
        string sourcePlatform,
        IEnumerable<OrderItem> orderItemCollection
    ) : base(tenantId, executionUser, sourcePlatform, correlationId)
    {
        Code = code;
        Date = date;
        Customer = customer;
        OrderItemCollection = orderItemCollection;
    }

    public record OrderItem
    {
        // Properties
        public int Sequence { get; }
        public string? Description { get; }
        public decimal Quantity { get; }
        public decimal UnityValue { get; }
        public Product Product { get; } = null!;

        // Constructors
        public OrderItem(
            int sequence,
            string? description,
            decimal quantity,
            decimal unityValue,
            Product product)
        {
            Sequence = sequence;
            Description = description;
            Quantity = quantity;
            UnityValue = unityValue;
            Product = product;
        }

    }
}
