using ShopNServe.AdminPanel.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ShopNServe.AdminPanel.Permissions;

public class AdminPanelPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AdminPanelPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AdminPanelPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AdminPanelResource>(name);
    }
}
