using ShopNServe.ProductCatalog.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ShopNServe.ProductCatalog;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(ProductCatalogEntityFrameworkCoreTestModule)
    )]
public class ProductCatalogDomainTestModule : AbpModule
{

}
