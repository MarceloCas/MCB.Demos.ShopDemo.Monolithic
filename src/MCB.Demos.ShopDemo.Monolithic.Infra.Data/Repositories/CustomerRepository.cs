using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModelRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories;

public class CustomerRepository
    : RepositoryBase,
    ICustomerRepository
{
    // Fields
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerMongoDbDataModelRepository _customerMongoDbDataModelRepository;

    // Constructors
    public CustomerRepository(
        IAdapter adapter,
        ICustomerFactory customerFactory,
        ICustomerMongoDbDataModelRepository customerMongoDbDataModelRepository
    ) : base(adapter)
    {
        _customerFactory = customerFactory;
        _customerMongoDbDataModelRepository = customerMongoDbDataModelRepository;
    }

    public async Task<bool> AddAsync(Domain.Entities.Customers.Customer aggregationRoot, CancellationToken cancellationToken)
    {
        await _customerMongoDbDataModelRepository.AddAsync(
            dataModel: Adapter.Adapt<CustomerMongoDbDataModel>(aggregationRoot)!,
            cancellationToken
        );

        return true;
    }
    public Task<bool> AddOrUpdateAsync(Domain.Entities.Customers.Customer aggregationRoot, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Domain.Entities.Customers.Customer> Get(Func<Domain.Entities.Customers.Customer, bool> expression)
    {
        throw new NotImplementedException();
    }
    public Task<Domain.Entities.Customers.Customer> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<Domain.Entities.Customers.Customer>> GetAsync(Func<Domain.Entities.Customers.Customer, bool> expression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Domain.Entities.Customers.Customer> GetAll(Guid tenantId, Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Domain.Entities.Customers.Customer>> GetAllAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(bool success, int removeCount)> RemoveAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(Domain.Entities.Customers.Customer aggregationRoot, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(bool success, int removeCount)> RemoveAsync(Func<Domain.Entities.Customers.Customer, bool> expression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Domain.Entities.Customers.Customer aggregationRoot, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Domain.Entities.Customers.Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var dataModel = (
            await _customerMongoDbDataModelRepository.FindAsync(dataModel => dataModel.Email == email, cancellationToken)
        ).FirstOrDefault();

        if (dataModel is null)
            return null;

        return _customerFactory.Create()!.SetExistingCustomerInfo(
            Adapter.Adapt<SetExistingCustomerInfoInput>(dataModel)!
        );
    }
}