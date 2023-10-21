using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ShopNServe.ProductCatalog.EntityFrameworkCore;

[ConnectionStringName(ProductCatalogDbProperties.ConnectionStringName)]
public class ProductCatalogDbContext : AbpDbContext<ProductCatalogDbContext>, IProductCatalogDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureProductCatalog();
    }
}
