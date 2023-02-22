using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications.Interfaces;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Validators;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Wrappers;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators;
public class ImportOrderInputShouldBeValid
    : InputBaseValidator<ImportOrderInput>,
    IImportOrderInputShouldBeValid
{
    // Fields
    private readonly IOrderSpecifications _orderSpecifications;

    // Constructors
    public ImportOrderInputShouldBeValid(
        IInputBaseSpecifications inputBaseSpecifications,
        IDateTimeProvider dateTimeProvider
    ) : base(inputBaseSpecifications)
    {
        _orderSpecifications = new OrderSpecifications(dateTimeProvider);
    }

    protected override void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        // Code
        OrderValidatorWrapper.AddOrderShouldHaveCode(
            _orderSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Code,
            getCodeFunction: input => input.Code
        );

        // Date
        OrderValidatorWrapper.AddOrderShouldHaveDate(
            _orderSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Date,
            getDateFunction: input => input.Date
        );
        OrderValidatorWrapper.AddOrderShouldHaveDateWithValidLength(
            _orderSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Date,
            getDateFunction: input => input.Date
        );

        // Customer
        OrderValidatorWrapper.AddOrderShouldHaveCustomer(
            _orderSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Customer,
            getCustomerFunction: input => input.Customer
        );

        // OrderItems
        OrderValidatorWrapper.AddOrderShouldHaveOrderItems(
            _orderSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input,
            getImportOrderInputFunction: input => input
        );
        OrderValidatorWrapper.AddOrderShouldHaveOrderItemsWithValidSequence(
            _orderSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input,
            getImportOrderInputFunction: input => input
        );
    }
}
