using ShopNServe.ProductCatalog.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ShopNServe.ProductCatalog.Permissions;

public class ProductCatalogPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ProductCatalogPermissions.GroupName, L("Permission:ProductCatalog"));

        var prodCat = myGroup.AddPermission(ProductCatalogPermissions.Products.Default, L("Permission:ProductCatalog"));
        prodCat.AddChild(ProductCatalogPermissions.Products.Delete, L("Permission:ProductCatalog:Delete"));
        prodCat.AddChild(ProductCatalogPermissions.Products.Update, L("Permission:ProductCatalog:Update"));
        prodCat.AddChild(ProductCatalogPermissions.Products.Create, L("Permission:ProductCatalog:Create"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProductCatalogResource>(name);
    }
}
