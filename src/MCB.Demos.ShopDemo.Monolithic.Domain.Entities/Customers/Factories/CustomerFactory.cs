using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories;

public sealed class CustomerFactory
    : ICustomerFactory
{
    // Fields
    private readonly IDateTimeProvider _dateTimeProvider;

    // Constructors
    public CustomerFactory(
        IDateTimeProvider dateTimeProvider
    )
    {
        _dateTimeProvider = dateTimeProvider;
    }

    // Public Methods
    public Customer? Create()
    {
        return new Customer(_dateTimeProvider);
    }
}