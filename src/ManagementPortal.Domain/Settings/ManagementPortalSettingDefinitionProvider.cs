using Volo.Abp.Settings;

namespace ManagementPortal.Settings;

public class ManagementPortalSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ManagementPortalSettings.MySetting1));
    }
}

