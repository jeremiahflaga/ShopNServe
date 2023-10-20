using Volo.Abp.Modularity;

namespace ShopNServe.AuthServer;

[DependsOn(
    typeof(AuthServerApplicationModule),
    typeof(AuthServerDomainTestModule)
    )]
public class AuthServerApplicationTestModule : AbpModule
{

}
