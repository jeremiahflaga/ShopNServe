using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace ShopNServe.ProductCatalog;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ProductCatalogHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class ProductCatalogConsoleApiClientModule : AbpModule
{

}
