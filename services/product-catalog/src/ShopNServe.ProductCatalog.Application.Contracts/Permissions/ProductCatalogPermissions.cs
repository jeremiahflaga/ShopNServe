using Volo.Abp.Reflection;

namespace ShopNServe.ProductCatalog.Permissions;

public class ProductCatalogPermissions
{
    public const string GroupName = "ProductCatalog";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProductCatalogPermissions));
    }
}
