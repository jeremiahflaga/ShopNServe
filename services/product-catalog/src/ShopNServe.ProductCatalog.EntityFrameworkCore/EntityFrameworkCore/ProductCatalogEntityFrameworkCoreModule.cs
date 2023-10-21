using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ShopNServe.ProductCatalog.EntityFrameworkCore;

[DependsOn(
    typeof(ProductCatalogDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ProductCatalogEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ProductCatalogDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });
    }
}
