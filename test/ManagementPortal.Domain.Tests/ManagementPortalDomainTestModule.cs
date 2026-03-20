using Volo.Abp.Modularity;

namespace ManagementPortal;

[DependsOn(
    typeof(ManagementPortalDomainModule),
    typeof(ManagementPortalTestBaseModule)
)]
public class ManagementPortalDomainTestModule : AbpModule
{

}

