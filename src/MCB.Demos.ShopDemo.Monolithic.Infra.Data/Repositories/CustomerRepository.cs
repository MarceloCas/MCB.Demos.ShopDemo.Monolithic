using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories;

public class CustomerRepository
    : RepositoryBase,
    ICustomerRepository
{
    // Constants
    public const string GET_BY_EMAIL_TRACE_NAME = $"{nameof(CustomerRepository)}.{nameof(GetByEmailAsync)}";
    public const string IMPORT_CUSTOMER_TRACE_NAME = $"{nameof(CustomerRepository)}.{nameof(ImportCustomerAsync)}";

    // Fields
    private readonly TimeSpan _customerDataModelTTL;
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerDataModelEntityFrameworkRepository _customerDataModelRepository;
    private readonly ICustomerDataModelRedisRepository _customerDataModelRedisRepository;

    // Constructors
    public CustomerRepository(
        ITraceManager traceManager,
        IAdapter adapter,
        AppSettings appSettings,
        ICustomerFactory customerFactory,
        ICustomerDataModelEntityFrameworkRepository customerDataModelRepository,
        ICustomerDataModelRedisRepository customerDataModelRedisRepository
    ) : base(traceManager, adapter)
    {
        _customerDataModelTTL = TimeSpan.FromSeconds(appSettings.Redis.TTLSeconds.CustomerDataModel);
        _customerFactory = customerFactory;
        _customerDataModelRepository = customerDataModelRepository;
        _customerDataModelRedisRepository = customerDataModelRedisRepository;
    }

    public Task<Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: GET_BY_EMAIL_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: tenantId,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: (
                TenantId: tenantId, 
                Email: email, 
                CustomerDataModelRepository: _customerDataModelRepository,
                CustomerDataModelRedisRepository: _customerDataModelRedisRepository,
                CustomerFactory: _customerFactory, 
                Adapter
            ),
            handler: async (input, activity, cancellationToken) =>
            {
                var customerDataModel = await input.CustomerDataModelRedisRepository.GetAsync(
                    input.CustomerDataModelRedisRepository.GetKey(input.TenantId, input.Email)
                );

                if (customerDataModel != null)
                    return input.CustomerFactory.Create()!.SetExistingCustomerInfo(
                        input.Adapter.Adapt<SetExistingCustomerInfoInput>(customerDataModel)!
                    );

                customerDataModel = await input.CustomerDataModelRepository.GetByEmailAsync(
                    input.TenantId,
                    input.Email,
                    cancellationToken
                );

                if (customerDataModel != null)
                    await input.CustomerDataModelRedisRepository.AddOrUpdateAsync(
                        customerDataModel, 
                        expiry: _customerDataModelTTL, 
                        cancellationToken
                    );

                return input.CustomerFactory.Create()!.SetExistingCustomerInfo(
                    input.Adapter.Adapt<SetExistingCustomerInfoInput>(customerDataModel)!
                );
            },
            cancellationToken
        )!;
    }

    public Task<(bool Success, int ModifiedCount)> ImportCustomerAsync(Customer customer, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: IMPORT_CUSTOMER_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: customer.TenantId,
            executionUser: customer.AuditableInfo.CreatedBy,
            sourcePlatform: customer.AuditableInfo.LastSourcePlatform,
            input: (Customer: customer, Adapter, CustomerDataModelRepository: _customerDataModelRepository),
            handler: async (input, activity, cancellationToken) =>
            {
                var customerDataModel = input.Adapter.Adapt<Customer, CustomerDataModel>(input.Customer);

                if (customerDataModel is null)
                    return default;

                var entry = await input.CustomerDataModelRepository.AddAsync(customerDataModel, cancellationToken);

                return (Success: entry.IsKeySet, ModifiedCount: 1);
            },
            cancellationToken
        )!;
    }
}