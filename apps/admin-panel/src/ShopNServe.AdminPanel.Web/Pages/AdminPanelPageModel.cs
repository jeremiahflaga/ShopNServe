using ShopNServe.AdminPanel.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ShopNServe.AdminPanel.Web.Pages;

public abstract class AdminPanelPageModel : AbpPageModel
{
    protected AdminPanelPageModel()
    {
        LocalizationResourceType = typeof(AdminPanelResource);
    }
}
