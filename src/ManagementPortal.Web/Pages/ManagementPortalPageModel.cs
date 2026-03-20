using ManagementPortal.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ManagementPortal.Web.Pages;

public abstract class ManagementPortalPageModel : AbpPageModel
{
    protected ManagementPortalPageModel()
    {
        LocalizationResourceType = typeof(ManagementPortalResource);
    }
}

