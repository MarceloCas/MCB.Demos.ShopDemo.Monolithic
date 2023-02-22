using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Interfaces;

public interface ICustomerService
    : IService<Customer>
{
    Task<(bool Success, Customer? ImportedCustomer)> ImportCustomerAsync(ImportCustomerServiceInput input, CancellationToken cancellationToken);
    Task<(bool Success, IEnumerable<Notification>? NotificationCollection)> ValidateImportCustomerAsync(ValidateImportCustomerServiceInput input, CancellationToken cancellationToken);
    Task<(bool Success, Customer? RemovedCustomer)> DeleteCustomerAsync(DeleteCustomerServiceInput input, CancellationToken cancellationToken);
}