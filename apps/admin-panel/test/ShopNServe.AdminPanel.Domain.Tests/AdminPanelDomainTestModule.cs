using ShopNServe.AdminPanel.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ShopNServe.AdminPanel;

[DependsOn(
    typeof(AdminPanelEntityFrameworkCoreTestModule)
    )]
public class AdminPanelDomainTestModule : AbpModule
{

}
