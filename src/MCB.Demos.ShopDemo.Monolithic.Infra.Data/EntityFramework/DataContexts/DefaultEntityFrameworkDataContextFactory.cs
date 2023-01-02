using Microsoft.EntityFrameworkCore.Design;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;

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
