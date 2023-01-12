using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.EntityTypeConfigurations;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;

public class DefaultEntityFrameworkDataContext
    : EntityFrameworkDataContextBase
{
    // Fields
    private readonly IPostgreSqlResiliencePolicy _postgreSqlResiliencePolicy;

    // Constructors
    public DefaultEntityFrameworkDataContext(
        ITraceManager traceManager,
        string connectionString,
        IPostgreSqlResiliencePolicy postgreSqlResiliencePolicy
    ) : base(traceManager, connectionString)
    {
        _postgreSqlResiliencePolicy = postgreSqlResiliencePolicy;
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
        return _postgreSqlResiliencePolicy.ExecuteAsync(
            handler: base.CommitTransactionAsync,
            cancellationToken
        );
    }
}

public class DefaultEntityFrameworkDataContextFactory
    : IDesignTimeDbContextFactory<DefaultEntityFrameworkDataContext>
{
    public DefaultEntityFrameworkDataContext CreateDbContext(string[] args)
    {
        return new DefaultEntityFrameworkDataContext(
            traceManager: null!,
            connectionString: args[0],
            postgreSqlResiliencePolicy: null!
        );
    }
}
