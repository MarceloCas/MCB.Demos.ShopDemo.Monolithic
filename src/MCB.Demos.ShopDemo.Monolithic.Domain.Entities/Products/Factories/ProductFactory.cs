using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Factories;
public class ProductFactory
    : IProductFactory
{
    // Fields
    private readonly IDateTimeProvider _dateTimeProvider;

    // Constructors
    public ProductFactory(
        IDateTimeProvider dateTimeProvider
    )
    {
        _dateTimeProvider = dateTimeProvider;
    }

    // Public Methods
    public Product? Create()
    {
        return new Product(_dateTimeProvider);
    }
}
