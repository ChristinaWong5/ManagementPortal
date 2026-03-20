using Volo.Abp.Modularity;

namespace ManagementPortal;

public abstract class ManagementPortalApplicationTestBase<TStartupModule> : ManagementPortalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

