using ManagementPortal.Samples;
using Xunit;

namespace ManagementPortal.EntityFrameworkCore.Domains;

[Collection(ManagementPortalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ManagementPortalEntityFrameworkCoreTestModule>
{

}

