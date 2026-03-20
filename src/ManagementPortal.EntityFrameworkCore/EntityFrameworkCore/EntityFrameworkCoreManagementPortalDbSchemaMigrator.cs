using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ManagementPortal.Data;
using Volo.Abp.DependencyInjection;

namespace ManagementPortal.EntityFrameworkCore;

public class EntityFrameworkCoreManagementPortalDbSchemaMigrator
    : IManagementPortalDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreManagementPortalDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ManagementPortalDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ManagementPortalDbContext>()
            .Database
            .MigrateAsync();
    }
}

