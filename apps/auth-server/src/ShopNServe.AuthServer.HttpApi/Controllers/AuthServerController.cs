using ShopNServe.AuthServer.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopNServe.AuthServer.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AuthServerController : AbpControllerBase
{
    protected AuthServerController()
    {
        LocalizationResource = typeof(AuthServerResource);
    }
}
