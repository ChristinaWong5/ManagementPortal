using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ManagementPortal.Data;

/* This is used if database provider does't define
 * IManagementPortalDbSchemaMigrator implementation.
 */
public class NullManagementPortalDbSchemaMigrator : IManagementPortalDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}

