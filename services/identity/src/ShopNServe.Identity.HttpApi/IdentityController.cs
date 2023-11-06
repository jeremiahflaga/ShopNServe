using ShopNServe.Identity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopNServe.Identity;

public abstract class IdentityController : AbpControllerBase
{
    protected IdentityController()
    {
        LocalizationResource = typeof(IdentityResource);
    }
}
