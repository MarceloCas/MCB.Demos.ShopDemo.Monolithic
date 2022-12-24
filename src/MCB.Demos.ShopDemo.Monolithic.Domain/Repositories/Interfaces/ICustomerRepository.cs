using MCB.Core.Domain.Abstractions.Repositories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;

public interface ICustomerRepository
    : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken);
    Task<(bool Success, int ModifiedCount)> RegisterNewCustomerAsync(Customer customer, CancellationToken cancellationToken);
}