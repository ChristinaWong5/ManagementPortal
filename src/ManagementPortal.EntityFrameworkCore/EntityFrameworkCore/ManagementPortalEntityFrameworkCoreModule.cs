using ManagementPortal.DownloaderWebSockets;
using ManagementPortal.Downloaders;
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Abp.Gdpr;
using Volo.Abp.Studio;

namespace ManagementPortal.EntityFrameworkCore;

[DependsOn(typeof(ManagementPortalDomainModule), typeof(AbpPermissionManagementEntityFrameworkCoreModule), typeof(AbpSettingManagementEntityFrameworkCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpBackgroundJobsEntityFrameworkCoreModule), typeof(AbpAuditLoggingEntityFrameworkCoreModule), typeof(AbpFeatureManagementEntityFrameworkCoreModule), typeof(AbpIdentityProEntityFrameworkCoreModule), typeof(AbpOpenIddictProEntityFrameworkCoreModule), typeof(LanguageManagementEntityFrameworkCoreModule), typeof(FileManagementEntityFrameworkCoreModule), typeof(TextTemplateManagementEntityFrameworkCoreModule), typeof(AbpGdprEntityFrameworkCoreModule), typeof(BlobStoringDatabaseEntityFrameworkCoreModule))]
public class ManagementPortalEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ManagementPortalEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ManagementPortalDbContext>(options => {
            /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            options.AddRepository<Downloader, Downloaders.EfCoreDownloaderRepository>();
            options.AddRepository<DownloaderWebSocket, DownloaderWebSockets.EfCoreDownloaderWebSocketRepository>();
        });
        if (AbpStudioAnalyzeHelper.IsInAnalyzeMode)
        {
            return;
        }

        Configure<AbpDbContextOptions>(options => {
            /* The main point to change your DBMS.
             * See also ManagementPortalDbContextFactory for EF Core tooling. */
            options.UseSqlite();
        });
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
        Configure<AbpUnitOfWorkDefaultOptions>(options => {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });
    }
}
