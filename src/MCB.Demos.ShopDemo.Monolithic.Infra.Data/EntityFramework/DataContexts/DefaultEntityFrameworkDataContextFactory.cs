using Microsoft.EntityFrameworkCore.Design;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts;

public class DefaultEntityFrameworkDataContextFactory
    : IDesignTimeDbContextFactory<DefaultEntityFrameworkDataContext>
{
    public DefaultEntityFrameworkDataContext CreateDbContext(string[] args)
    {
        return new DefaultEntityFrameworkDataContext(
            connectionString: "Host=localhost;Port=5432;Username=admin;Password=123456;Database=mcb_demos_shopdemo_monolithic"
        );
    }
}
