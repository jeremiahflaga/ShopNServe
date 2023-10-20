using Volo.Abp.Settings;

namespace ShopNServe.AdminPanel.Settings;

public class AdminPanelSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AdminPanelSettings.MySetting1));
    }
}
