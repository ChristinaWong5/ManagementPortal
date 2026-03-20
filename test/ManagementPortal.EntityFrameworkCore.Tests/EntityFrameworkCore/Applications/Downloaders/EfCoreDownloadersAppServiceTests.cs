using ManagementPortal.Downloaders;
using Xunit;
using ManagementPortal.EntityFrameworkCore;

namespace ManagementPortal.Downloaders;

public class EfCoreDownloadersAppServiceTests : DownloadersAppServiceTests<ManagementPortalEntityFrameworkCoreTestModule>
{
}
