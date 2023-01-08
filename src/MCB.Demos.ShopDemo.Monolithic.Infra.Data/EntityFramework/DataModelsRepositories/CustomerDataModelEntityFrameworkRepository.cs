using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories;

public class CustomerDataModelEntityFrameworkRepository
    : EntityFrameworkDataModelRepositoryBase<CustomerDataModel>,
    ICustomerDataModelEntityFrameworkRepository
{
    // Constants
    public const string GET_BY_EMAIL_TRACE_NAME = $"{nameof(CustomerDataModelEntityFrameworkRepository)}.{nameof(GetByEmailAsync)}.{nameof(GetAsync)}";

    // Constructors
    public CustomerDataModelEntityFrameworkRepository(
        IEntityFrameworkDataContext entityFrameworkDataContext,
        ITraceManager traceManager
    ) : base(entityFrameworkDataContext, traceManager)
    {
    }

    // Public Methods
    public Task<CustomerDataModel?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: GET_BY_EMAIL_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: tenantId,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: (TenantId: tenantId, Email: email),
            handler: (input, activity, cancellationToken) =>
            {
                return GetFirstOrDefaultAsync(q => q.TenantId == input.TenantId && q.Email == input.Email, cancellationToken);
            },
            cancellationToken
        );
        
    }
}
