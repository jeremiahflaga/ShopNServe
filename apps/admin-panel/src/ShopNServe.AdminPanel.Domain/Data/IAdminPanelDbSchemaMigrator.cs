using System.Threading.Tasks;

namespace ShopNServe.AdminPanel.Data;

public interface IAdminPanelDbSchemaMigrator
{
    Task MigrateAsync();
}
