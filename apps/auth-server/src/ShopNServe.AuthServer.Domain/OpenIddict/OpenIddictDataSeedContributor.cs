﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace ShopNServe.AuthServer.OpenIddict;

/* Creates initial data that is needed to property run the application
 * and make client-to-server communication possible.
 */
public class OpenIddictDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IConfiguration _configuration;
    private readonly IOpenIddictApplicationRepository _openIddictApplicationRepository;
    private readonly IAbpApplicationManager _applicationManager;
    private readonly IOpenIddictScopeRepository _openIddictScopeRepository;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IStringLocalizer<OpenIddictResponse> L;

    public OpenIddictDataSeedContributor(
        IConfiguration configuration,
        IOpenIddictApplicationRepository openIddictApplicationRepository,
        IAbpApplicationManager applicationManager,
        IOpenIddictScopeRepository openIddictScopeRepository,
        IOpenIddictScopeManager scopeManager,
        IPermissionDataSeeder permissionDataSeeder,
        IStringLocalizer<OpenIddictResponse> l )
    {
        _configuration = configuration;
        _openIddictApplicationRepository = openIddictApplicationRepository;
        _applicationManager = applicationManager;
        _openIddictScopeRepository = openIddictScopeRepository;
        _scopeManager = scopeManager;
        _permissionDataSeeder = permissionDataSeeder;
        L = l;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateScopesAsync();
        await CreateApplicationsAsync();
    }

    private async Task CreateScopesAsync()
    {
        if (await _openIddictScopeRepository.FindByNameAsync("AuthServer") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor {
                Name = "AuthServer", DisplayName = "AuthServer API", Resources = { "AuthServer" }
            });
        }
        if (await _openIddictScopeRepository.FindByNameAsync("AdminPanel") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "AdminPanel",
                DisplayName = "AdminPanel Web App",
                Resources = { "AdminPanel" }
            });
        }
        if (await _openIddictScopeRepository.FindByNameAsync("ProductCatalog") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "ProductCatalog",
                DisplayName = "ProductCatalog Swagger",
                Resources = { "ProductCatalog" }
            });
        }
        if (await _openIddictScopeRepository.FindByNameAsync("Identity") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "Identity",
                DisplayName = "Identity Swagger",
                Resources = { "Identity" }
            });
        }
    }

    private async Task CreateApplicationsAsync()
    {
        var commonScopes = new List<string> {
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles,
            "AuthServer",
        };

        var configurationSection = _configuration.GetSection("OpenIddict:Applications");

        ////Web Client
        //var webClientId = configurationSection["AuthServer_Web:ClientId"];
        //if (!webClientId.IsNullOrWhiteSpace())
        //{
        //    var webClientRootUrl = configurationSection["AuthServer_Web:RootUrl"]!.EnsureEndsWith('/');

        //    /* AuthServer_Web client is only needed if you created a tiered
        //     * solution. Otherwise, you can delete this client. */
        //    await CreateApplicationAsync(
        //        name: webClientId!,
        //        type: OpenIddictConstants.ClientTypes.Confidential,
        //        consentType: OpenIddictConstants.ConsentTypes.Implicit,
        //        displayName: "Web Application",
        //        secret: configurationSection["AuthServer_Web:ClientSecret"] ?? "1q2w3e*",
        //        grantTypes: new List<string> //Hybrid flow
        //        {
        //            OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit
        //        },
        //        scopes: commonScopes,
        //        redirectUri: $"{webClientRootUrl}signin-oidc",
        //        clientUri: webClientRootUrl,
        //        postLogoutRedirectUri: $"{webClientRootUrl}signout-callback-oidc"
        //    );
        //}

        ////Console Test / Angular Client
        //var consoleAndAngularClientId = configurationSection["AuthServer_App:ClientId"];
        //if (!consoleAndAngularClientId.IsNullOrWhiteSpace())
        //{
        //    var consoleAndAngularClientRootUrl = configurationSection["AuthServer_App:RootUrl"]?.TrimEnd('/');
        //    await CreateApplicationAsync(
        //        name: consoleAndAngularClientId!,
        //        type: OpenIddictConstants.ClientTypes.Public,
        //        consentType: OpenIddictConstants.ConsentTypes.Implicit,
        //        displayName: "Console Test / Angular Application",
        //        secret: null,
        //        grantTypes: new List<string> {
        //            OpenIddictConstants.GrantTypes.AuthorizationCode,
        //            OpenIddictConstants.GrantTypes.Password,
        //            OpenIddictConstants.GrantTypes.ClientCredentials,
        //            OpenIddictConstants.GrantTypes.RefreshToken
        //        },
        //        scopes: commonScopes,
        //        redirectUri: consoleAndAngularClientRootUrl,
        //        clientUri: consoleAndAngularClientRootUrl,
        //        postLogoutRedirectUri: consoleAndAngularClientRootUrl
        //    );
        //}

        //// Blazor Client
        //var blazorClientId = configurationSection["AuthServer_Blazor:ClientId"];
        //if (!blazorClientId.IsNullOrWhiteSpace())
        //{
        //    var blazorRootUrl = configurationSection["AuthServer_Blazor:RootUrl"]?.TrimEnd('/');

        //    await CreateApplicationAsync(
        //        name: blazorClientId!,
        //        type: OpenIddictConstants.ClientTypes.Public,
        //        consentType: OpenIddictConstants.ConsentTypes.Implicit,
        //        displayName: "Blazor Application",
        //        secret: null,
        //        grantTypes: new List<string> { OpenIddictConstants.GrantTypes.AuthorizationCode, },
        //        scopes: commonScopes,
        //        redirectUri: $"{blazorRootUrl}/authentication/login-callback",
        //        clientUri: blazorRootUrl,
        //        postLogoutRedirectUri: $"{blazorRootUrl}/authentication/logout-callback"
        //    );
        //}

        //// Blazor Server Tiered Client
        //var blazorServerTieredClientId = configurationSection["AuthServer_BlazorServerTiered:ClientId"];
        //if (!blazorServerTieredClientId.IsNullOrWhiteSpace())
        //{
        //    var blazorServerTieredRootUrl = configurationSection["AuthServer_BlazorServerTiered:RootUrl"]!.EnsureEndsWith('/');

        //    await CreateApplicationAsync(
        //        name: blazorServerTieredClientId!,
        //        type: OpenIddictConstants.ClientTypes.Confidential,
        //        consentType: OpenIddictConstants.ConsentTypes.Implicit,
        //        displayName: "Blazor Server Application",
        //        secret: configurationSection["AuthServer_BlazorServerTiered:ClientSecret"] ?? "1q2w3e*",
        //        grantTypes: new List<string> //Hybrid flow
        //        {
        //            OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit
        //        },
        //        scopes: commonScopes,
        //        redirectUri: $"{blazorServerTieredRootUrl}signin-oidc",
        //        clientUri: blazorServerTieredRootUrl,
        //        postLogoutRedirectUri: $"{blazorServerTieredRootUrl}signout-callback-oidc"
        //    );
        //}

        // Swagger Client
        var swaggerClientId = configurationSection["AuthServer_Swagger:ClientId"];
        if (!swaggerClientId.IsNullOrWhiteSpace())
        {
            var swaggerRootUrl = configurationSection["AuthServer_Swagger:RootUrl"]?.TrimEnd('/');

            await CreateApplicationAsync(
                name: swaggerClientId!,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Swagger Application",
                secret: null,
                grantTypes: new List<string> { OpenIddictConstants.GrantTypes.AuthorizationCode, },
                scopes: commonScopes,
                redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html",
                clientUri: swaggerRootUrl
            );
        }

        // Admin Panel Web Client
        var adminPanelWebClientId = configurationSection["AdminPanel_Web:ClientId"];
        if (!adminPanelWebClientId.IsNullOrWhiteSpace())
        {
            var webClientRootUrl = configurationSection["AdminPanel_Web:RootUrl"]!.EnsureEndsWith('/');
            await CreateApplicationAsync(
                name: adminPanelWebClientId!,
                type: OpenIddictConstants.ClientTypes.Confidential,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Admin Panel Web Application",
                secret: configurationSection["AdminPanel_Web:ClientSecret"] ?? "1q2w3e*",
                grantTypes: new List<string> //Hybrid flow
                {
                    OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit
                },
                scopes: new List<string>(commonScopes) { "AdminPanel", "ProductCatalog", "Identity" },
                redirectUri: $"{webClientRootUrl}signin-oidc",
                clientUri: webClientRootUrl,
                postLogoutRedirectUri: $"{webClientRootUrl}signout-callback-oidc"
            );
        }

        await CreateProductCatalogSwaggerClientApplicationAsync(commonScopes, configurationSection);
        await CreateIdentityServiceSwaggerClientApplicationAsync(commonScopes, configurationSection);
    }

    private async Task CreateProductCatalogSwaggerClientApplicationAsync(List<string> commonScopes, IConfigurationSection configurationSection)
    {
        // Product Catalog Service Swagger Client
        var productCatalogSwaggerClientId = configurationSection["ProductCatalog_Swagger:ClientId"];
        if (!productCatalogSwaggerClientId.IsNullOrWhiteSpace())
        {
            var swaggerRootUrl = configurationSection["ProductCatalog_Swagger:RootUrl"]?.TrimEnd('/');

            await CreateApplicationAsync(
                name: productCatalogSwaggerClientId!,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Product Catalog Service Swagger Application",
                secret: null,
                grantTypes: new List<string> { OpenIddictConstants.GrantTypes.AuthorizationCode, },
                scopes: new List<string>(commonScopes) { "ProductCatalog" },
                redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html",
                clientUri: swaggerRootUrl
            );
        }
    }

    private async Task CreateIdentityServiceSwaggerClientApplicationAsync(List<string> commonScopes, IConfigurationSection configurationSection)
    {
        // Identity Service Swagger Client
        var identityServiceSwaggerClientId = configurationSection["Identity_Swagger:ClientId"];
        if (!identityServiceSwaggerClientId.IsNullOrWhiteSpace())
        {
            var swaggerRootUrl = configurationSection["Identity_Swagger:RootUrl"]?.TrimEnd('/');

            await CreateApplicationAsync(
                name: identityServiceSwaggerClientId!,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Identity Service Swagger Application",
                secret: null,
                grantTypes: new List<string> { OpenIddictConstants.GrantTypes.AuthorizationCode, },
                scopes: new List<string>(commonScopes) { "Identity" },
                redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html",
                clientUri: swaggerRootUrl
            );
        }
    }

    private async Task CreateApplicationAsync(
        [NotNull] string name,
        [NotNull] string type,
        [NotNull] string consentType,
        string displayName,
        string? secret,
        List<string> grantTypes,
        List<string> scopes,
        string? clientUri = null,
        string? redirectUri = null,
        string? postLogoutRedirectUri = null,
        List<string>? permissions = null)
    {
        if (!string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Public,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException(L["NoClientSecretCanBeSetForPublicApplications"]);
        }

        if (string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Confidential,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException(L["TheClientSecretIsRequiredForConfidentialApplications"]);
        }

        var client = await _openIddictApplicationRepository.FindByClientIdAsync(name);

        var application = new AbpApplicationDescriptor {
            ClientId = name,
            Type = type,
            ClientSecret = secret,
            ConsentType = consentType,
            DisplayName = displayName,
            ClientUri = clientUri,
        };

        Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));
        Check.NotNullOrEmpty(scopes, nameof(scopes));

        if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(
                grantTypes.Contains))
        {
            application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

            if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
            }
        }

        if (!redirectUri.IsNullOrWhiteSpace() || !postLogoutRedirectUri.IsNullOrWhiteSpace())
        {
            application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
        }

        var buildInGrantTypes = new[] {
            OpenIddictConstants.GrantTypes.Implicit, OpenIddictConstants.GrantTypes.Password,
            OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.ClientCredentials,
            OpenIddictConstants.GrantTypes.DeviceCode, OpenIddictConstants.GrantTypes.RefreshToken
        };

        foreach (var grantType in grantTypes)
        {
            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
            }

            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
            }

            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
                grantType == OpenIddictConstants.GrantTypes.Password ||
                grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
                grantType == OpenIddictConstants.GrantTypes.DeviceCode)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
            }

            if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Password)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
            }

            if (grantType == OpenIddictConstants.GrantTypes.RefreshToken)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
            }

            if (grantType == OpenIddictConstants.GrantTypes.DeviceCode)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                }
            }

            if (!buildInGrantTypes.Contains(grantType))
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.GrantType + grantType);
            }
        }

        var buildInScopes = new[] {
            OpenIddictConstants.Permissions.Scopes.Address, OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone, OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles
        };

        foreach (var scope in scopes)
        {
            if (buildInScopes.Contains(scope))
            {
                application.Permissions.Add(scope);
            }
            else
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
            }
        }

        if (redirectUri != null)
        {
            if (!redirectUri.IsNullOrEmpty())
            {
                if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                {
                    throw new BusinessException(L["InvalidRedirectUri", redirectUri]);
                }

                if (application.RedirectUris.All(x => x != uri))
                {
                    application.RedirectUris.Add(uri);
                }
            }
        }

        if (postLogoutRedirectUri != null)
        {
            if (!postLogoutRedirectUri.IsNullOrEmpty())
            {
                if (!Uri.TryCreate(postLogoutRedirectUri, UriKind.Absolute, out var uri) ||
                    !uri.IsWellFormedOriginalString())
                {
                    throw new BusinessException(L["InvalidPostLogoutRedirectUri", postLogoutRedirectUri]);
                }

                if (application.PostLogoutRedirectUris.All(x => x != uri))
                {
                    application.PostLogoutRedirectUris.Add(uri);
                }
            }
        }

        if (permissions != null)
        {
            await _permissionDataSeeder.SeedAsync(
                ClientPermissionValueProvider.ProviderName,
                name,
                permissions,
                null
            );
        }

        if (client == null)
        {
            await _applicationManager.CreateAsync(application);
            return;
        }

        if (!HasSameRedirectUris(client, application))
        {
            client.RedirectUris = JsonSerializer.Serialize(application.RedirectUris.Select(q => q.ToString().TrimEnd('/')));
            client.PostLogoutRedirectUris = JsonSerializer.Serialize(application.PostLogoutRedirectUris.Select(q => q.ToString().TrimEnd('/')));

            await _applicationManager.UpdateAsync(client.ToModel());
        }

        if (!HasSameScopes(client, application))
        {
            client.Permissions = JsonSerializer.Serialize(application.Permissions.Select(q => q.ToString()));
            await _applicationManager.UpdateAsync(client.ToModel());
        }
    }

    private bool HasSameRedirectUris(OpenIddictApplication existingClient, AbpApplicationDescriptor application)
    {
        return existingClient.RedirectUris == JsonSerializer.Serialize(application.RedirectUris.Select(q => q.ToString().TrimEnd('/')));
    }

    private bool HasSameScopes(OpenIddictApplication existingClient, AbpApplicationDescriptor application)
    {
        return existingClient.Permissions == JsonSerializer.Serialize(application.Permissions.Select(q => q.ToString().TrimEnd('/')));
    }
}
