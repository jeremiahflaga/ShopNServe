using Volo.Abp.Modularity;

namespace ShopNServe.AdminPanel;

[DependsOn(
    typeof(AdminPanelApplicationModule),
    typeof(AdminPanelDomainTestModule)
    )]
public class AdminPanelApplicationTestModule : AbpModule
{

}
