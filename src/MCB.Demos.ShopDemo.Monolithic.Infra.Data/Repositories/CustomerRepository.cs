using Mapster;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories;

public class CustomerRepository
    : RepositoryBase,
    ICustomerRepository
{
    // Fields
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerDataModelRepository _customerDataModelRepository;

    // Constructors
    public CustomerRepository(
        IAdapter adapter,
        ICustomerFactory customerFactory,
        ICustomerDataModelRepository customerDataModelRepository
    ) : base(adapter)
    {
        _customerFactory = customerFactory;
        _customerDataModelRepository = customerDataModelRepository;
    }

    public Task<bool> AddAsync(Customer aggregationRoot, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
    public Task<bool> AddOrUpdateAsync(Customer aggregationRoot, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Customer> Get(Func<Customer, bool> expression)
    {
        throw new NotImplementedException();
    }
    public Task<Customer> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<Customer>> GetAsync(Func<Customer, bool> expression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Customer> GetAll(Guid tenantId, Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Customer>> GetAllAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(bool success, int removeCount)> RemoveAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(Customer aggregationRoot, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(bool success, int removeCount)> RemoveAsync(Func<Customer, bool> expression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Customer aggregationRoot, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var customerDataModel = (await _customerDataModelRepository.GetAsync(q => q.Email == email, cancellationToken)).FirstOrDefault();

        return customerDataModel is null
            ? null
            : _customerFactory.Create()!.SetExistingCustomerInfo(
                Adapter.Adapt<SetExistingCustomerInfoInput>(customerDataModel)!
            );
    }

    public async Task<(bool Success, int ModifiedCount)> RegisterNewCustomerAsync(Customer customer, CancellationToken cancellationToken)
    {
        var customerDataModel = customer.Adapt<CustomerDataModel>();

        var entry = await _customerDataModelRepository.AddAsync(customerDataModel, cancellationToken);

        return (Success: entry.IsKeySet, ModifiedCount: 1);
    }
}