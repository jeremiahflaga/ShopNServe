using ShopNServe.ProductCatalog.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopNServe.ProductCatalog;

public abstract class ProductCatalogController : AbpControllerBase
{
    protected ProductCatalogController()
    {
        LocalizationResource = typeof(ProductCatalogResource);
    }
}
