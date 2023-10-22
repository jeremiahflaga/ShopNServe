using Volo.Abp.Reflection;

namespace ShopNServe.ProductCatalog.Permissions;

public class ProductCatalogPermissions
{
    public const string GroupName = "ProductCatalog";

    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Delete = Default + ".Delete";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProductCatalogPermissions));
    }
}
