using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ShopNServe.AdminPanel.Data;

/* This is used if database provider does't define
 * IAdminPanelDbSchemaMigrator implementation.
 */
public class NullAdminPanelDbSchemaMigrator : IAdminPanelDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
