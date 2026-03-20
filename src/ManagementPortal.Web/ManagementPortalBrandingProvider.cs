using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using ManagementPortal.Localization;

namespace ManagementPortal.Web;

[Dependency(ReplaceServices = true)]
public class ManagementPortalBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ManagementPortalResource> _localizer;

    public ManagementPortalBrandingProvider(IStringLocalizer<ManagementPortalResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}

