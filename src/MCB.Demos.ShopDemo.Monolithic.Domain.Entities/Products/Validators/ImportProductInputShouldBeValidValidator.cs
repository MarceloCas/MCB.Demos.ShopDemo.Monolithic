using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications.Interfaces;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Validators;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Validators.Wrappers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Specifications;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Validators.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Validators;
public sealed class ImportProductInputShouldBeValidValidator
    : InputBaseValidator<ImportProductInput>,
    IImportProductInputShouldBeValidValidator
{
    // Fields
    private readonly IProductSpecifications _productSpecifications;

    // Constructors
    public ImportProductInputShouldBeValidValidator(
        IInputBaseSpecifications inputBaseSpecifications,
        IDateTimeProvider dateTimeProvider
    ) : base(inputBaseSpecifications)
    {
        _productSpecifications = new ProductSpecifications(dateTimeProvider);
    }

    // Protected Methods
    protected override void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        // Code
        ProductValidatorWrapper.AddProductShouldHaveCode(
            _productSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Code,
            getCodeFunction: input => input.Code
        );
        ProductValidatorWrapper.AddProductShouldHaveCodeMaximumLength(
            _productSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Code,
            getCodeFunction: input => input.Code
        );

        // Description
        ProductValidatorWrapper.AddProductShouldHaveDescription(
            _productSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Description,
            getDescriptionFunction: input => input.Description
        );
        ProductValidatorWrapper.AddProductShouldHaveDescriptionMaximumLength(
            _productSpecifications,
            fluentValidationValidatorWrapper,
            propertyExpression: input => input.Description,
            getDescriptionFunction: input => input.Description
        );
    }
}
