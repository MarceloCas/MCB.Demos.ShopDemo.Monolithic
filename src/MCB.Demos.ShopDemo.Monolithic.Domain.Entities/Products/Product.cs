using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Validators;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
public class Product
    : DomainEntityBase,
    IAggregationRoot
{
    // Properties
    public string Code { get; private set; }
    public string? Description { get; private set; }

    // Validators
    private readonly ImportProductInputShouldBeValidValidator _importProductInputShouldBeValidValidator;

    // Constructors
    public Product(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
        Code = string.Empty;

        _importProductInputShouldBeValidValidator = new ImportProductInputShouldBeValidValidator(
            new InputBaseSpecifications(),
            dateTimeProvider
        );
    }

    // Public Methods
    public Product ImportProduct(ImportProductInput input)
    {
        // Validate
        if (!Validate(() => _importProductInputShouldBeValidValidator.Validate(input)))
            return this;

        // Process and Return
        return SetProductInfo(
            input.Code,
            input.Description
        )
        .RegisterNewInternal<Product>(input.TenantId, input.ExecutionUser, input.SourcePlatform, input.CorrelationId);
    }
    public Product SetExistingProductInfo(SetExistingProductInfoInput input)
    {
        SetExistingInfoInternal<Product>(
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

        SetProductInfo(input.Code, input.Description);

        return this;
    }

    public Product DeepClone()
    {
        // Process and Return
        return DeepCloneInternal<Product>()
            .SetProductInfo(Code, Description);
    }

    // Protected Abstract Methods
    protected override DomainEntityBase CreateInstanceForCloneInternal() => new Product(DateTimeProvider);

    // Private Methods
    private Product SetProductInfo(string code, string? description)
    {
        Code = code;
        Description = description;

        return this;
    }
}
