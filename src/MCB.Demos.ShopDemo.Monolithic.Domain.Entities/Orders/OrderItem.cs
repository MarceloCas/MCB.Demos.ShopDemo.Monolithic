using MCB.Core.Domain.Entities.DomainEntitiesBase;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders;
public class OrderItem
    : DomainEntityBase
{
    // Fields
    private readonly RegisterNewOrderItemInputShouldBeValidValidator _registerNewOrderItemInputShouldBeValidValidator;

    // Properties
    public int Sequence { get; private set; }
    public string? Description { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnityValue { get; private set; }
    public Product Product { get; private set; } = null!;

    // Constructors
    public OrderItem(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
        _registerNewOrderItemInputShouldBeValidValidator = new RegisterNewOrderItemInputShouldBeValidValidator(
            new InputBaseSpecifications(),
            dateTimeProvider
        );
    }

    // Public Methods
    public OrderItem RegisterNewOrderItem(RegisterNewOrderItemInput input)
    {
        // Validate
        if (!Validate(() => _registerNewOrderItemInputShouldBeValidValidator.Validate(input)))
            return this;

        // Process and Return
        return SetOrderItemInfo(
            input.Sequence,
            input.Description,
            input.Quantity,
            input.UnityValue,
            input.Product
        )
        .RegisterNewInternal<OrderItem>(input.TenantId, input.ExecutionUser, input.SourcePlatform, input.CorrelationId);
    }
    public OrderItem SetExistingOrderItemInfo(SetExistingOrderItemInfoInput input)
    {
        SetExistingInfoInternal<OrderItem>(
            input.Id,
            input.TenantId,
            input.CreatedBy,
            input.CreatedAt,
            input.LastUpdatedBy,
            input.LastUpdatedAt,
            input.LastSourcePlatform,
            input.RegistryVersion,
            input.CorrelationId
        );

        SetOrderItemInfo(
            input.Sequence,
            input.Description,
            input.Quantity,
            input.UnityValue,
            input.Product
        );

        return this;
    }

    public OrderItem DeepClone()
    {
        // Process and Return
        return DeepCloneInternal<OrderItem>()
            .SetOrderItemInfo(
                Sequence,
                Description,
                Quantity,
                UnityValue,
                Product
            );
    }

    // Protected Methods
    protected override DomainEntityBase CreateInstanceForCloneInternal() => new OrderItem(DateTimeProvider);

    // Private Methods
    private OrderItem SetOrderItemInfo(
        int sequence,
        string? description,
        decimal quantity,
        decimal unityValue,
        Product product
    )
    {
        Sequence = sequence;
        Description = description;
        Quantity = quantity;
        UnityValue = unityValue;
        Product = product;

        return this;
    }
}
