using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ShopNServe.ProductCatalog;

[DependsOn(
    typeof(ProductCatalogApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ProductCatalogHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ProductCatalogApplicationContractsModule).Assembly,
            ProductCatalogRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProductCatalogHttpApiClientModule>();
        });

    }
}
