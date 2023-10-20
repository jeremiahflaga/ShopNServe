using System;
using System.Collections.Generic;
using System.Text;
using ShopNServe.AdminPanel.Localization;
using Volo.Abp.Application.Services;

namespace ShopNServe.AdminPanel;

/* Inherit your application services from this class.
 */
public abstract class AdminPanelAppService : ApplicationService
{
    protected AdminPanelAppService()
    {
        LocalizationResource = typeof(AdminPanelResource);
    }
}
