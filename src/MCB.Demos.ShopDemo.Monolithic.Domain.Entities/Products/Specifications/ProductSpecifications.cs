using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Specifications.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Specifications;

public class ProductSpecifications
    : DomainEntitySpecifications,
    IProductSpecifications
{
    // Constructors
    public ProductSpecifications(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Public Methods
    public bool ProductShouldHaveCode(string firstName)
    {
        return !string.IsNullOrEmpty(firstName);
    }
    public bool ProductShouldHaveCodeMaximumLength(string firstName)
    {
        return firstName.Length <= IProductSpecifications.PRODUCT_CODE_MAX_LENGTH;
    }

    public bool ProductShouldHaveDescription(string lastName)
    {
        return !string.IsNullOrEmpty(lastName);
    }
    public bool ProductShouldHaveDescriptionMaximumLength(string lastName)
    {
        return lastName.Length <= IProductSpecifications.PRODUCT_DESCRIPTION_MAX_LENGTH;
    }
}
