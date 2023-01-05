using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts;
public class DefaultEntityFrameworkDataContext
    : EntityFrameworkDataContextBase
{
    // Constructors
    public DefaultEntityFrameworkDataContext(
        ITraceManager traceManager,
        string connectionString
    ) : base(traceManager, connectionString)
    {

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
}

public class DefaultEntityFrameworkDataContextFactory
    : IDesignTimeDbContextFactory<DefaultEntityFrameworkDataContext>
{
    public DefaultEntityFrameworkDataContext CreateDbContext(string[] args)
    {
        return new DefaultEntityFrameworkDataContext(
            traceManager: null!,
            connectionString: args[0]
        );
    }
}
