using System.Threading.Tasks;

namespace ManagementPortal.Data;

public interface IManagementPortalDbSchemaMigrator
{
    Task MigrateAsync();
}

