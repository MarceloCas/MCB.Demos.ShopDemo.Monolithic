using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications.Interfaces;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Validators;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators;
public class ImportOrderInputShouldBeValid
    : InputBaseValidator<ImportOrderInput>,
    IImportOrderInputShouldBeValid
{
    // Fields
    private readonly IOrderItemSpecifications _orderItemSpecifications;

    // Constructors
    public ImportOrderInputShouldBeValid(
        IInputBaseSpecifications inputBaseSpecifications,
        IDateTimeProvider dateTimeProvider
    ) : base(inputBaseSpecifications)
    {
        _orderItemSpecifications = new OrderItemSpecifications(dateTimeProvider);
    }

    protected override void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {

    }
}
