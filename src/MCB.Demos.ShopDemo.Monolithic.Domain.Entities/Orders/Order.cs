using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders;
public class Order
    : DomainEntityBase,
    IAggregationRoot
{
    // Fields
    private readonly List<OrderItem> _orderItemCollection;

    private readonly IOrderItemFactory _orderItemFactory;

    // Properties
    public string Code { get; private set; }
    public DateTime Date { get; private set; }
    public Customers.Customer Customer { get; private set; } = null!;
    public IEnumerable<OrderItem> OrderItemCollection => _orderItemCollection.AsReadOnly().Select(q => q.DeepClone());

    // Constructors
    public Order(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
        Code = string.Empty;
        _orderItemCollection = new List<OrderItem>();
        _orderItemFactory = new OrderItemFactory(dateTimeProvider);
    }

    // Public Methods
    public Order ImportOrder(ImportOrderInput input)
    {
        // Validate input

        // Process order

        // Process itens
        foreach (var orderItem in input.OrderItemCollection)
        {
            AddOrderItem(
                input.CorrelationId,
                orderItem.Sequence,
                orderItem.Description,
                orderItem.Quantity,
                orderItem.UnityValue,
                orderItem.Product,
                input.ExecutionUser,
                input.SourcePlatform
            );

            // Validate after insert item
            if (!ValidationInfo.IsValid)
                return this;
        }

        return this;
    }

    // Protected Methods
    protected override DomainEntityBase CreateInstanceForCloneInternal() => new Order(DateTimeProvider);

    // Private Methods
    private void SetOrderInfo(
        string code,
        DateTime date,
        Customers.Customer customer
    )
    {
        Code = code;
        Date = date;
        Customer = customer;
    }
    private Order AddOrderItem(
        Guid correlationId,
        int sequence,
        string? description,
        decimal quantity,
        decimal unityValue,
        Product product,
        string executionUser,
        string sourcePlatform
    )
    {
        // Validate pré-process
        // e.g. Sequence order, product already exists

        // Process
        var orderItem = _orderItemFactory.Create()!;

        orderItem.RegisterNewOrderItem(
            new Inputs.RegisterNewOrderItemInput(
                TenantId,
                sequence,
                description,
                quantity,
                unityValue,
                product,
                executionUser,
                sourcePlatform,
                correlationId
            )
        );

        // Validate after register new order item
        if (!Validate(() => orderItem.ValidationInfo))
            return this;

        _orderItemCollection.Add(orderItem);

        return this;
    }
}
