using ShopNServe.ProductCatalog.Localization;
using Volo.Abp.Application.Services;

namespace ShopNServe.ProductCatalog;

public abstract class ProductCatalogAppService : ApplicationService
{
    protected ProductCatalogAppService()
    {
        LocalizationResource = typeof(ProductCatalogResource);
        ObjectMapperContext = typeof(ProductCatalogApplicationModule);
    }
}
