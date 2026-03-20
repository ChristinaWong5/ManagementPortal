using ManagementPortal.Localization;
using Volo.Abp.Application.Services;

namespace ManagementPortal;

/* Inherit your application services from this class.
 */
public abstract class ManagementPortalAppService : ApplicationService
{
    protected ManagementPortalAppService()
    {
        LocalizationResource = typeof(ManagementPortalResource);
    }
}

