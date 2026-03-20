using ManagementPortal.Samples;
using Xunit;

namespace ManagementPortal.EntityFrameworkCore.Applications;

[Collection(ManagementPortalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ManagementPortalEntityFrameworkCoreTestModule>
{

}

