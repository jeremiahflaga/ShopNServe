using Volo.Abp.Modularity;

namespace ShopNServe.Identity;

[DependsOn(
    typeof(IdentityApplicationModule),
    typeof(IdentityDomainTestModule)
    )]
public class IdentityApplicationTestModule : AbpModule
{

}
