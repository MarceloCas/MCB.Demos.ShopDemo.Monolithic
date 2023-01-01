using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
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
        ITraceManager traceManager,
        IAdapter adapter,
        ICustomerFactory customerFactory,
        ICustomerDataModelRepository customerDataModelRepository
    ) : base(traceManager, adapter)
    {
        _customerFactory = customerFactory;
        _customerDataModelRepository = customerDataModelRepository;
    }

    public Task<Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: $"{nameof(CustomerRepository)}.{nameof(GetByEmailAsync)}",
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: tenantId,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: (TenantId: tenantId, Email: email, CustomerDataModelRepository: _customerDataModelRepository, CustomerFactory: _customerFactory, Adapter),
            handler: async (input, activity, cancellationToken) =>
            {
                var customerDataModel = (
                    await input.CustomerDataModelRepository.GetAsync(q =>
                        q.TenantId == input.TenantId
                        && q.Email == input.Email,
                        cancellationToken
                    )
                ).FirstOrDefault();

                return customerDataModel is null
                    ? null
                    : input.CustomerFactory.Create()!.SetExistingCustomerInfo(
                        input.Adapter.Adapt<SetExistingCustomerInfoInput>(customerDataModel)!
                    );
            },
            cancellationToken
        )!;
    }

    public Task<(bool Success, int ModifiedCount)> ImportCustomerAsync(Customer customer, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: $"{nameof(CustomerRepository)}.{nameof(ImportCustomerAsync)}",
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