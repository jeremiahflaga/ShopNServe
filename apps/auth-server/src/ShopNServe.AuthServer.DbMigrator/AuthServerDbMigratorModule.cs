using ShopNServe.AuthServer.EntityFrameworkCore;
using ShopNServe.Identity.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace ShopNServe.AuthServer.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AuthServerEntityFrameworkCoreModule),
    typeof(AuthServerApplicationContractsModule),
    typeof(SnSIdentityEntityFrameworkCoreModule)
    )]
public class AuthServerDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "AuthServer:"; });
    }
}
