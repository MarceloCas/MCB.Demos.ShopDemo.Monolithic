using MCB.Core.Domain.Abstractions.Repositories;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;

public interface ICustomerRepository
    : IRepository<Entities.Customers.Customer>
{
    Task<Entities.Customers.Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}