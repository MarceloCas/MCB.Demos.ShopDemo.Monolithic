using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;

public interface ICustomerService
    : IService<Entities.Customers.Customer>
{
    Task<bool> RegisterNewCustomerAsync(RegisterNewCustomerServiceInput input, CancellationToken cancellationToken);
}