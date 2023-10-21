using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace ShopNServe.ProductCatalog.MongoDB;

[DependsOn(
    typeof(ProductCatalogDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class ProductCatalogMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<ProductCatalogMongoDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
