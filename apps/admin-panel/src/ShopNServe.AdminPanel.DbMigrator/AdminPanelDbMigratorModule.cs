using ShopNServe.AdminPanel.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace ShopNServe.AdminPanel.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AdminPanelEntityFrameworkCoreModule),
    typeof(AdminPanelApplicationContractsModule)
    )]
public class AdminPanelDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "AdminPanel:"; });
    }
}
