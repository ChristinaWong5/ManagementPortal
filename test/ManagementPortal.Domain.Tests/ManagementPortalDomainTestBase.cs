using Volo.Abp.Modularity;

namespace ManagementPortal;

/* Inherit from this class for your domain layer tests. */
public abstract class ManagementPortalDomainTestBase<TStartupModule> : ManagementPortalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

