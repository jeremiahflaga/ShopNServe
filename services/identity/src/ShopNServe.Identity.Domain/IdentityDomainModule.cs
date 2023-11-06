using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace ShopNServe.Identity;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(IdentityDomainSharedModule),
    typeof(AbpIdentityDomainModule) // AbpIdentityDomainModule is the one which seeds data for AbpUsers table
)]
public class IdentityDomainModule : AbpModule
{

}
