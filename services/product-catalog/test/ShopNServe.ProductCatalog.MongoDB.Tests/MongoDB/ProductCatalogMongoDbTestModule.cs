using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace ShopNServe.ProductCatalog.MongoDB;

[DependsOn(
    typeof(ProductCatalogTestBaseModule),
    typeof(ProductCatalogMongoDbModule)
    )]
public class ProductCatalogMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
