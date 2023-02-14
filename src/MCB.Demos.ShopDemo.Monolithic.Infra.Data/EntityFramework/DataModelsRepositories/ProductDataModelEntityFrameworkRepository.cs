using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories;
public class ProductDataModelEntityFrameworkRepository
    : EntityFrameworkDataModelRepositoryBase<ProductDataModel>,
    IProductDataModelEntityFrameworkRepository
{
    // Constants
    public const string GET_BY_CODE_TRACE_NAME = $"{nameof(ProductDataModelEntityFrameworkRepository)}.{nameof(GetByCodeAsync)}.{nameof(GetAsync)}";

    // Constructors
    public ProductDataModelEntityFrameworkRepository(
        IEntityFrameworkDataContext entityFrameworkDataContext,
        ITraceManager traceManager,
        IPostgreSqlResiliencePolicy postgreSqlResiliencePolicy
    ) : base(entityFrameworkDataContext, traceManager, postgreSqlResiliencePolicy)
    {

    }

    // Public Methods
    public Task<ProductDataModel?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: GET_BY_CODE_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: tenantId,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: (TenantId: tenantId, Code: code),
            handler: async (input, activity, cancellationToken) =>
            {
                var result = await PostgreSqlResiliencePolicy.ExecuteAsync(
                    handler: (input, cancellationToken) => GetFirstOrDefaultAsync(q => q.TenantId == input.TenantId && q.Code == input.Code, cancellationToken),
                    input: input,
                    cancellationToken
                );

                return result.Output;
            },
            cancellationToken
        );
    }
}
