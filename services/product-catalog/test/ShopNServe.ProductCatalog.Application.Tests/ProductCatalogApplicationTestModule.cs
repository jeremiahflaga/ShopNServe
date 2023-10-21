using Volo.Abp.Modularity;

namespace ShopNServe.ProductCatalog;

[DependsOn(
    typeof(ProductCatalogApplicationModule),
    typeof(ProductCatalogDomainTestModule)
    )]
public class ProductCatalogApplicationTestModule : AbpModule
{

}
