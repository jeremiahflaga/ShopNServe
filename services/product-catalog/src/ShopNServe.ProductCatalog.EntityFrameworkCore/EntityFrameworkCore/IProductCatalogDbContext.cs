using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ShopNServe.ProductCatalog.EntityFrameworkCore;

[ConnectionStringName(ProductCatalogDbProperties.ConnectionStringName)]
public interface IProductCatalogDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
