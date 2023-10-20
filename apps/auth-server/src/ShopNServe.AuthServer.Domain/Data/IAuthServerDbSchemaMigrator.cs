using System.Threading.Tasks;

namespace ShopNServe.AuthServer.Data;

public interface IAuthServerDbSchemaMigrator
{
    Task MigrateAsync();
}
