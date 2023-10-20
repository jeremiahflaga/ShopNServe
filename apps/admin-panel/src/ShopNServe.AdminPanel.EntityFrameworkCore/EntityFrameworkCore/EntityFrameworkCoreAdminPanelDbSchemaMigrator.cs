using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopNServe.AdminPanel.Data;
using Volo.Abp.DependencyInjection;

namespace ShopNServe.AdminPanel.EntityFrameworkCore;

public class EntityFrameworkCoreAdminPanelDbSchemaMigrator
    : IAdminPanelDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAdminPanelDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the AdminPanelDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AdminPanelDbContext>()
            .Database
            .MigrateAsync();
    }
}
