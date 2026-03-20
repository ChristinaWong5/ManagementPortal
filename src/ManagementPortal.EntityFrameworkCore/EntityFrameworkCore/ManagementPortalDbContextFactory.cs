using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ManagementPortal.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ManagementPortalDbContextFactory : IDesignTimeDbContextFactory<ManagementPortalDbContext>
{
    public ManagementPortalDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        ManagementPortalEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<ManagementPortalDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        
        return new ManagementPortalDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ManagementPortal.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}

