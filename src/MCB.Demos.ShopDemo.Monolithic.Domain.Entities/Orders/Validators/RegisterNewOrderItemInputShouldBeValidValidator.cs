using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications.Interfaces;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Validators;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Wrappers;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators;
public class RegisterNewOrderItemInputShouldBeValidValidator
    : InputBaseValidator<RegisterNewOrderItemInput>,
    IRegisterNewOrderItemInputShouldBeValidValidator
{
    // Fields
    private readonly IOrderItemSpecifications _orderItemSpecifications;

    // Constructors
    public RegisterNewOrderItemInputShouldBeValidValidator(
        IInputBaseSpecifications inputBaseSpecifications,
        IDateTimeProvider dateTimeProvider
    ) : base(inputBaseSpecifications)
    {
        _orderItemSpecifications = new OrderItemSpecifications(dateTimeProvider);
    }

    // Protected Methods
    protected override void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        // Sequence
        OrderItemValidatorWrapper.AddOrderItemShouldHaveSequence(
            _orderItemSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Sequence,
            getSequenceFunction: input => input.Sequence
        );

        // Description
        OrderItemValidatorWrapper.AddOrderItemShouldHaveDescriptionMaximumLength(
            _orderItemSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Description,
            getDescriptionFunction: input => input.Description
        );

        // Quantity
        OrderItemValidatorWrapper.AddOrderItemShouldHaveQuantity(
            _orderItemSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Quantity,
            getQuantityFunction: input => input.Quantity
        );

        // UnityValue
        OrderItemValidatorWrapper.AddOrderItemShouldHaveUnityValue(
            _orderItemSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.UnityValue,
            getUnityValueFunction: input => input.UnityValue
        );

        // Product
        OrderItemValidatorWrapper.AddOrderItemShouldHaveProduct(
            _orderItemSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Product,
            getProductFunction: input => input.Product
        );
    }
}