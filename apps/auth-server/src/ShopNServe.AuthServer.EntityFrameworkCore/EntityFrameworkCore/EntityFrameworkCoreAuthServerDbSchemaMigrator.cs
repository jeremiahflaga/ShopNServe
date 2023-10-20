using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopNServe.AuthServer.Data;
using Volo.Abp.DependencyInjection;

namespace ShopNServe.AuthServer.EntityFrameworkCore;

public class EntityFrameworkCoreAuthServerDbSchemaMigrator
    : IAuthServerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAuthServerDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the AuthServerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AuthServerDbContext>()
            .Database
            .MigrateAsync();
    }
}
