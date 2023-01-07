using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModelsRepositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
        CustomerDataModelEntityFrameworkRepository customerDataModelEntityFrameworkRepository,
        IJsonSerializer jsonSerializer
    ) : base(redisDataContext, customerDataModelEntityFrameworkRepository, jsonSerializer)
    {
        _customerDataModelEntityFrameworkRepository = customerDataModelEntityFrameworkRepository;
    }

    // Private Methods

    private string GetKey(Guid tenantId, string email)
    {
        return $"mcb|customer|{tenantId}|{email}";
    }

    // Protected Methods
    protected override string? GetKey(CustomerDataModel? dataModel)
    {
        return dataModel is null ? null : GetKey(dataModel.TenantId, dataModel.Email);
    }

    // Public Methods
    public async Task<CustomerDataModel?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken)
    {
        await RedisDataContext.TryOpenConnectionAsync(cancellationToken);
        var key = GetKey(tenantId, email);
        var cacheItem = (await RedisDataContext.StringGetAsync(key)).ToString();

        CustomerDataModel? customerDataModel;
        if (string.IsNullOrWhiteSpace(cacheItem))
        {
            customerDataModel = await _customerDataModelEntityFrameworkRepository.GetByEmailAsync(tenantId, email, cancellationToken);

            if (customerDataModel != null)
                await AddAsync(customerDataModel, cancellationToken);
        }
        else
            customerDataModel = JsonSerializer.DeserializeFromJson<CustomerDataModel>(cacheItem!);

        return customerDataModel;
    }
}
