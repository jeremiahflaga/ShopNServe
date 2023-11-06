using ShopNServe.Identity.Localization;
using Volo.Abp.Application.Services;

namespace ShopNServe.Identity;

public abstract class IdentityAppService : ApplicationService
{
    protected IdentityAppService()
    {
        LocalizationResource = typeof(IdentityResource);
        ObjectMapperContext = typeof(IdentityApplicationModule);
    }
}
