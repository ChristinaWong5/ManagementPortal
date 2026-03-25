using ManagementPortal.DownloaderWebSockets;
using ManagementPortal.Downloaders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Abp.Gdpr;

namespace ManagementPortal.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ConnectionStringName("Default")]
public class ManagementPortalDbContext : AbpDbContext<ManagementPortalDbContext>, IIdentityProDbContext
{
    public DbSet<DownloaderWebSocket> DownloaderWebSockets { get; set; } = null!;
    public DbSet<Downloader> Downloaders { get; set; } = null!;

    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    #region Entities from the modules
    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */
    // Identity
    public DbSet<IdentityUser> Users { get; set; }

    public DbSet<IdentityRole> Roles { get; set; }

    public DbSet<IdentityClaimType> ClaimTypes { get; set; }

    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }

    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    public DbSet<IdentitySession> Sessions { get; set; }
    #endregion
    public ManagementPortalDbContext(DbContextOptions<ManagementPortalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        /* Include modules to your migration db context */
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureLanguageManagement();
        builder.ConfigureFileManagement();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureGdpr();
        builder.ConfigureBlobStoring();
        /* Configure your own tables/entities inside here */
        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ManagementPortalConsts.DbTablePrefix + "YourEntities", ManagementPortalConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<DownloaderWebSocket>(b => {
                b.ToTable(ManagementPortalConsts.DbTablePrefix + "DownloaderWebSockets", ManagementPortalConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Host).HasColumnName(nameof(DownloaderWebSocket.Host));
                b.Property(x => x.Port).HasColumnName(nameof(DownloaderWebSocket.Port));
                b.HasOne<Downloader>().WithMany(x => x.DownloaderWebSockets).HasForeignKey(x => x.DownloaderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Downloader>(b => {
                b.ToTable(ManagementPortalConsts.DbTablePrefix + "Downloaders", ManagementPortalConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.DownloaderEnabled).HasColumnName(nameof(Downloader.DownloaderEnabled));
                b.Property(x => x.DownloaderPollarName).HasColumnName(nameof(Downloader.DownloaderPollarName));
                b.Property(x => x.DownstreamHealthFile).HasColumnName(nameof(Downloader.DownstreamHealthFile));
                b.HasMany(x => x.DownloaderWebSockets).WithOne().HasForeignKey(x => x.DownloaderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
