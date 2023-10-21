using Localization.Resources.AbpUi;
using ShopNServe.ProductCatalog.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ShopNServe.ProductCatalog;

[DependsOn(
    typeof(ProductCatalogApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ProductCatalogHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ProductCatalogHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ProductCatalogResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
