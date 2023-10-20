using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ShopNServe.AdminPanel.Web;

[Dependency(ReplaceServices = true)]
public class AdminPanelBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "AdminPanel";
}
