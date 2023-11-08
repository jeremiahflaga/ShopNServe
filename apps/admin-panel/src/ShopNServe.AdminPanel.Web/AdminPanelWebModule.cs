using System;
using System.IO;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ShopNServe.AdminPanel.Localization;
using ShopNServe.AdminPanel.MultiTenancy;
using ShopNServe.AdminPanel.Web.Menus;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Http.Client.Web;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using ShopNServe.AuthServer;
using Polly;
using ShopNServe.ProductCatalog;
using Volo.Abp.Http.Client;
using System.Net.Http.Headers;
using Volo.Abp.Http.Client.Authentication;

namespace ShopNServe.AdminPanel.Web;

[DependsOn(
    typeof(AuthServerHttpApiClientModule),
    typeof(ProductCatalogHttpApiClientModule),
    //typeof(AdminPanelHttpApiModule),
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
    typeof(AbpAspNetCoreMvcClientModule),
    typeof(AbpHttpClientWebModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpDistributedLockingModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class AdminPanelWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AdminPanelResource),
                typeof(AdminPanelDomainSharedModule).Assembly,
                //typeof(AdminPanelApplicationContractsModule).Assembly,
                typeof(AdminPanelWebModule).Assembly
            );
        });

        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((remoteServiceName, clientBuilder) =>
            {
                clientBuilder.AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(
                        3,
                        i => TimeSpan.FromSeconds(Math.Pow(2, i))
                    )
                );
            });

            // Adding Bearer Access Token and disable SSL validation for Dynamic Proxies: https://support.abp.io/QA/Questions/2566/Adding-Bearer-Access-Token-and-disable-SSL-validation-for-Dynamic-Proxies
            // Question: Dynamic C# Client Authorization: https://github.com/abpframework/abp/issues/7551
            // Dynamic C# API Client Proxies Unable to get jwt in header: https://github.com/abpframework/abp/issues/14509
            options.ProxyClientActions.Add(async (name, serviceProvider, httpClient) =>
            {
                var accessTokenProvider = serviceProvider.GetService<IAbpAccessTokenProvider>();
                if (accessTokenProvider != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                        //await accessTokenProvider.GetTokenAsync());
                        "eyJhbGciOiJSUzI1NiIsImtpZCI6IjBDQkJCNDJCM0I0QUMyMkZBMDIwRUMyNTIwQjUwQzgxNTUyNTkwNzIiLCJ4NXQiOiJETHUwS3p0S3dpLWdJT3dsSUxVTWdWVWxrSEkiLCJ0eXAiOiJhdCtqd3QifQ.eyJzdWIiOiIzYTBlYzA1ZC1mZDM2LTlhZGEtNDhjMy1lMmM0YTJlMmQ5ZWQiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJhZG1pbiIsImVtYWlsIjoiYWRtaW5AYWJwLmlvIiwicm9sZSI6ImFkbWluIiwiZ2l2ZW5fbmFtZSI6ImFkbWluIiwicGhvbmVfbnVtYmVyX3ZlcmlmaWVkIjoiRmFsc2UiLCJlbWFpbF92ZXJpZmllZCI6IkZhbHNlIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsIm9pX3Byc3QiOiJQcm9kdWN0Q2F0YWxvZ19Td2FnZ2VyIiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTEvIiwib2lfYXVfaWQiOiIzYTBlYzA3Yi02OWFjLTExODEtOWNkZC1mNDU4NjRiYjk1YTYiLCJjbGllbnRfaWQiOiJQcm9kdWN0Q2F0YWxvZ19Td2FnZ2VyIiwib2lfdGtuX2lkIjoiM2EwZWMyNjktOWVjMS1jODYxLTM2NTgtYzY1MTMzMDU2ZjZlIiwiYXVkIjoiUHJvZHVjdENhdGFsb2ciLCJzY29wZSI6IlByb2R1Y3RDYXRhbG9nIiwianRpIjoiNWY4MjUxYzMtZjMwNS00YWQ2LThhMTMtODY3NDEyOTk2NmZjIiwiZXhwIjoxNjk5NDcyNDU0LCJpYXQiOjE2OTk0Njg4NTR9.lBp5ne_ipvRwaseYkbkBUPNJE_P7h-lL9-hvcznAbtYfOc-F9E67SWesXiMX2b0JPQyjrZTP4SvBU4qPZN205nBvbgVfT9axzrxs7UvqSDC0FX1eWL6VwHf3RXHNmSuR9rxTbyoqfhrzXABUTAbUvb5Xu9jl2rptvdhq8l6Puj_g0u_8NJz9ad4vrzDH_vqO1w_BXR8rWsXfixvV0GUhQxDrMGq7ZDzD0iSbIcpYAdhEYsDFWGJbstnKsvTKBgZHMsiHlNgPbmUAarf82Qv8zOb8vvetjpOYjFsZpnDNgfCWIpV64XilEcCemyKLym2EAZTMK-ByrO7KFM2wPHI6Yg");
                }
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureBundles();
        ConfigureCache();
        ConfigureDataProtection(context, configuration, hostingEnvironment);
        ConfigureDistributedLocking(context, configuration);
        ConfigureUrls(configuration);
        ConfigureAuthentication(context, configuration);
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices(configuration);
        ConfigureMultiTenancy();
        ConfigureSwaggerServices(context.Services);
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureCache()
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "AdminPanel:";
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureMultiTenancy()
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.CheckTokenExpiration();
            })
            .AddAbpOpenIdConnect("oidc", options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                options.ClientId = configuration["AuthServer:ClientId"];
                options.ClientSecret = configuration["AuthServer:ClientSecret"];

                options.UsePkce = true;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("roles");
                options.Scope.Add("email");
                options.Scope.Add("phone");
                options.Scope.Add("AuthServer");
                options.Scope.Add("AdminPanel");
            });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AdminPanelWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<AdminPanelDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ShopNServe.AdminPanel.Domain.Shared"));
                //options.FileSets.ReplaceEmbeddedByPhysical<AdminPanelApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ShopNServe.AdminPanel.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<AdminPanelWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureNavigationServices(IConfiguration configuration)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AdminPanelMenuContributor(configuration));
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new AdminPanelToolbarContributor());
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AdminPanel API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    private void ConfigureDataProtection(
        ServiceConfigurationContext context,
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment)
    {
        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("AdminPanel");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "AdminPanel-Protection-Keys");
        }
    }

    private void ConfigureDistributedLocking(
        ServiceConfigurationContext context,
        IConfiguration configuration)
    {
        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminPanel API");
        });
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
