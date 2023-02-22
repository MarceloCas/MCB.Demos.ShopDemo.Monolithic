using MCB.Core.Domain.Abstractions.Repositories;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
public interface IOrderRepository
    : IRepository<Order>
{
    Task<Order?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken);
    Task<(bool Success, int ModifiedCount)> ImportOrderAsync(Order order, CancellationToken cancellationToken);
    Task<bool> RemoveOrderAsync(Order order, CancellationToken cancellationToken);
}