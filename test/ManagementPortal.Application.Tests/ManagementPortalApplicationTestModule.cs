using Volo.Abp.Modularity;

namespace ManagementPortal;

[DependsOn(
    typeof(ManagementPortalApplicationModule),
    typeof(ManagementPortalDomainTestModule)
)]
public class ManagementPortalApplicationTestModule : AbpModule
{

}

