using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Identity;
//using Volo.Abp.Account;

namespace ShopNServe.Identity;

[DependsOn(
    typeof(IdentityDomainModule),
    typeof(IdentityApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpIdentityApplicationModule)
    //,
    //typeof(AbpAccountApplicationModule) // AbpAccountApplicationModule is the one which seeds data for AbpUsers table
    )]
public class IdentityApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<IdentityApplicationModule>(validate: true);
        });
    }
}
