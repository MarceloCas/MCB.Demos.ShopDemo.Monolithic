using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories;
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
        var customerDataModelEntryCollection = ChangeTracker.Entries<CustomerDataModel>();
        if (customerDataModelEntryCollection.Any())
        {
            var customerDataModelEntityFrameworkRepository = _dependencyInjectionContainer.Resolve<ICustomerDataModelEntityFrameworkRepository>()!;
            await customerDataModelEntityFrameworkRepository.WriteBulkAsync(customerDataModelEntryCollection.Select(q => q.Entity), cancellationToken);
        }
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
        // check if in bulk operation
        //return _postgreSqlResiliencePolicy.ExecuteAsync(
        //    handler: base.CommitTransactionAsync,
        //    cancellationToken
        //);
        return _postgreSqlResiliencePolicy.ExecuteAsync(
            handler: BulkInsertAsync,
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
