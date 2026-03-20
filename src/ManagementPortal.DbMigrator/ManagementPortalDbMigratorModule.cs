using ManagementPortal.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ManagementPortal.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ManagementPortalEntityFrameworkCoreModule),
    typeof(ManagementPortalApplicationContractsModule)
)]
public class ManagementPortalDbMigratorModule : AbpModule
{
}

