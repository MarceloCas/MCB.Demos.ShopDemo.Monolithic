using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.EntityTypeConfigurations;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;

public class PostgreSqlEntityFrameworkDataContext
    : EntityFrameworkDataContextBase
{
    // Fields
    private readonly IPostgreSqlResiliencePolicy _postgreSqlResiliencePolicy;
    private readonly IDependencyInjectionContainer _dependencyInjectionContainer;
    // Constructors
    public PostgreSqlEntityFrameworkDataContext(
        ITraceManager traceManager,
        string connectionString,
        IDependencyInjectionContainer dependencyInjectionContainer,
        IPostgreSqlResiliencePolicy postgreSqlResiliencePolicy
    ) : base(traceManager, connectionString)
    {
        _dependencyInjectionContainer = dependencyInjectionContainer;
        _postgreSqlResiliencePolicy = postgreSqlResiliencePolicy;
    }

    // Private Methods
    private async Task BulkInsertAsync(CancellationToken cancellationToken)
    {
        var customerDataModelEntityFrameworkRepository = _dependencyInjectionContainer.Resolve<ICustomerDataModelEntityFrameworkRepository>()!;
        await customerDataModelEntityFrameworkRepository.WriteBulkAsync(cancellationToken);
    }

    // Protected Methods
    protected override void OnConfiguringInternal(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    protected override void OnModelCreatingInternal(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
    }

    // Public Methods
    public override Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return IsBulkInsertOperation
            ? _postgreSqlResiliencePolicy.ExecuteAsync(
                handler: BulkInsertAsync,
                cancellationToken
            )
            : (Task)_postgreSqlResiliencePolicy.ExecuteAsync(
                handler: base.CommitTransactionAsync,
                cancellationToken
            );
    }
}

public class DefaultEntityFrameworkDataContextFactory
    : IDesignTimeDbContextFactory<PostgreSqlEntityFrameworkDataContext>
{
    public PostgreSqlEntityFrameworkDataContext CreateDbContext(string[] args)
    {
        return new PostgreSqlEntityFrameworkDataContext(
            traceManager: null!,
            connectionString: args[0],
            dependencyInjectionContainer: null!,
            postgreSqlResiliencePolicy: null!
        );
    }
}
