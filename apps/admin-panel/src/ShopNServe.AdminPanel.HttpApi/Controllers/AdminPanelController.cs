using ShopNServe.AdminPanel.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopNServe.AdminPanel.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AdminPanelController : AbpControllerBase
{
    protected AdminPanelController()
    {
        LocalizationResource = typeof(AdminPanelResource);
    }
}
