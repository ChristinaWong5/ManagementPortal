using Volo.Abp.Settings;

namespace ManagementPortal.Downloaders;

public class DownloaderSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition(DownloaderSettings.MaxWorker, defaultValue: "4"));
    }
}