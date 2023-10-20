using ShopNServe.AuthServer.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ShopNServe.AuthServer;

[DependsOn(
    typeof(AuthServerEntityFrameworkCoreTestModule)
    )]
public class AuthServerDomainTestModule : AbpModule
{

}
