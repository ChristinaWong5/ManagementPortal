using Xunit;

namespace ManagementPortal.EntityFrameworkCore;

[CollectionDefinition(ManagementPortalTestConsts.CollectionDefinitionName)]
public class ManagementPortalEntityFrameworkCoreCollection : ICollectionFixture<ManagementPortalEntityFrameworkCoreFixture>
{

}

