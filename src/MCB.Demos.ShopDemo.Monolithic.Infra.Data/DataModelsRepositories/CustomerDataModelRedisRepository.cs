using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories;
public class CustomerDataModelRedisRepository
    : RedisDataModelRepositoryBase<CustomerDataModel>,
    ICustomerDataModelRepository
{
    // Fields
    private readonly CustomerDataModelEntityFrameworkRepository _customerDataModelEntityFrameworkRepository;

    // Constructors
    public CustomerDataModelRedisRepository(
        IRedisDataContext redisDataContext,
        CustomerDataModelEntityFrameworkRepository customerDataModelEntityFrameworkRepository
    ) : base(redisDataContext, customerDataModelEntityFrameworkRepository)
    {
        _customerDataModelEntityFrameworkRepository = customerDataModelEntityFrameworkRepository;
    }

    // Public Methods
    public Task<CustomerDataModel?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken)
    {
        return _customerDataModelEntityFrameworkRepository.GetByEmailAsync(tenantId, email, cancellationToken);
    }
}
