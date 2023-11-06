using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ShopNServe.Identity.EntityFrameworkCore;

[DependsOn(
    typeof(IdentityDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule)
)]
public class SnSIdentityEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SnSIdentityDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                
            options.ReplaceDbContext<Volo.Abp.Identity.EntityFrameworkCore.IIdentityDbContext>();

            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }
}
