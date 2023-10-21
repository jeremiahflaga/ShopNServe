using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace ShopNServe.ProductCatalog;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ProductCatalogDomainSharedModule)
)]
public class ProductCatalogDomainModule : AbpModule
{

}
