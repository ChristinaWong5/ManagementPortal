using ManagementPortal.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ManagementPortal.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ManagementPortalController : AbpControllerBase
{
    protected ManagementPortalController()
    {
        LocalizationResource = typeof(ManagementPortalResource);
    }
}

