using System.IO;
using System.Threading.Tasks;
using ManagementPortal.Downloaders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ManagementPortal.EntityFrameworkCore;
using ManagementPortal.Localization;
using ManagementPortal.MultiTenancy;
using ManagementPortal.Permissions;
using ManagementPortal.Web.Menus;
using ManagementPortal.Web.HealthChecks;
using Microsoft.OpenApi;
using Volo.Abp;
using Volo.Abp.Studio;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.Autofac;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Identity.Web;
using Volo.Abp.FeatureManagement;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Commercial;
using Volo.Abp.Account.Admin.Web;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.ExternalProviders;
using Volo.Abp.Account.Pro.Public.Web.Shared;
using Volo.Abp.AuditLogging.Web;
using Volo.Abp.LanguageManagement;
using Volo.FileManagement.Web;
using Volo.Abp.TextTemplateManagement.Web;
using Volo.Abp.Gdpr.Web;
using Volo.Abp.Gdpr.Web.Extensions;
using Volo.Abp.OpenIddict.Pro.Web;
using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Identity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Studio.Client.AspNetCore;

namespace ManagementPortal.Web;

[DependsOn(typeof(ManagementPortalHttpApiModule), typeof(ManagementPortalApplicationModule), typeof(ManagementPortalEntityFrameworkCoreModule), typeof(AbpAutofacModule), typeof(AbpStudioClientAspNetCoreModule), typeof(AbpIdentityWebModule), typeof(AbpAspNetCoreMvcUiLeptonXThemeModule), typeof(AbpAccountPublicWebOpenIddictModule), typeof(AbpAuditLoggingWebModule), typeof(AbpAccountAdminWebModule), typeof(AbpOpenIddictProWebModule), typeof(LanguageManagementWebModule), typeof(FileManagementWebModule), typeof(TextTemplateManagementWebModule), typeof(AbpGdprWebModule), typeof(AbpFeatureManagementWebModule), typeof(AbpSwashbuckleModule), typeof(AbpAspNetCoreSerilogModule))]
public class ManagementPortalWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options => {
            options.AddAssemblyResource(typeof(ManagementPortalResource), typeof(ManagementPortalDomainModule).Assembly, typeof(ManagementPortalDomainSharedModule).Assembly, typeof(ManagementPortalApplicationModule).Assembly, typeof(ManagementPortalApplicationContractsModule).Assembly, typeof(ManagementPortalWebModule).Assembly);
        });
        PreConfigure<OpenIddictBuilder>(builder => {
            builder.AddValidation(options => {
                options.AddAudiences("ManagementPortal");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options => {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });
            PreConfigure<OpenIddictServerBuilder>(serverBuilder => {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", configuration["AuthServer:CertificatePassPhrase"]!);
                serverBuilder.SetIssuer(new Uri(configuration["AuthServer:Authority"]!));
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        if (!configuration.GetValue<bool>("App:DisablePII"))
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        }

        if (!configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata"))
        {
            Configure<OpenIddictServerAspNetCoreOptions>(options => {
                options.DisableTransportSecurityRequirement = true;
            });
            Configure<ForwardedHeadersOptions>(options => {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        ConfigureStudio(hostingEnvironment);
        ConfigureBundles(hostingEnvironment);
        ConfigureUrls(configuration);
        ConfigureHealthChecks(context);
        ConfigurePages(configuration);
        ConfigureImpersonation(context, configuration);
        ConfigureExternalProviders(context);
        ConfigureCookieConsent(context);
        ConfigureAuthentication(context);
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);
        ConfigureTheme();
        Configure<PermissionManagementOptions>(options => {
            options.IsDynamicPermissionStoreEnabled = true;
        });
    }

    private void ConfigureCookieConsent(ServiceConfigurationContext context)
    {
        context.Services.AddAbpCookieConsent(options => {
            options.IsEnabled = true;
            options.CookiePolicyUrl = "/CookiePolicy";
            options.PrivacyPolicyUrl = "/PrivacyPolicy";
        });
    }

    private void ConfigureTheme()
    {
        Configure<LeptonXThemeOptions>(options => {
            options.DefaultStyle = LeptonXStyleNames.System;
        });
        Configure<LeptonXThemeMvcOptions>(options => {
            options.ApplicationLayout = LeptonXMvcLayouts.SideMenu;
        });
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddManagementPortalHealthChecks();
    }

    private void ConfigureStudio(IHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsProduction())
        {
            Configure<AbpStudioClientOptions>(options => {
                options.IsLinkEnabled = false;
            });
        }
    }

    private void ConfigureBundles(IHostEnvironment hostingEnvironment)
    {
        Configure<AbpBundlingOptions>(options => {
            options.StyleBundles.Configure(LeptonXThemeBundles.Styles.Global, bundle => {
                bundle.AddFiles("/global-styles.css");
            });
            options.ScriptBundles.Configure(LeptonXThemeBundles.Scripts.Global, bundle => {
                bundle.AddFiles("/global-scripts.js");
                if (hostingEnvironment.IsDevelopment())
                {
                    bundle.AddFiles("/dev-login-helper.js");
                }
            });
        });
    }

    private void ConfigurePages(IConfiguration configuration)
    {
        Configure<RazorPagesOptions>(options => {
            options.Conventions.AuthorizePage("/HostDashboard", ManagementPortalPermissions.Dashboard.Host);
            options.Conventions.AuthorizePage("/Downloaders/Index", ManagementPortalPermissions.Downloaders.Default);
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options => {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options => {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureImpersonation(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.Configure<AbpIdentityWebOptions>(options => {
            options.EnableUserImpersonation = true;
        });
        context.Services.Configure<AbpAccountOptions>(options => {
            options.TenantAdminUserName = "admin";
            options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(options => {
            options.FileSets.AddEmbedded<ManagementPortalWebModule>();
            if (hostingEnvironment.IsDevelopment())
            {
                options.FileSets.ReplaceEmbeddedByPhysical<ManagementPortalDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}ManagementPortal.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<ManagementPortalDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}ManagementPortal.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<ManagementPortalApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}ManagementPortal.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<ManagementPortalApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}ManagementPortal.Application", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<ManagementPortalHttpApiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}ManagementPortal.HttpApi", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<ManagementPortalWebModule>(hostingEnvironment.ContentRootPath);
            }
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options => {
            options.MenuContributors.Add(new ManagementPortalMenuContributor());
        });
        Configure<AbpToolbarOptions>(options => {
            options.Contributors.Add(new ManagementPortalToolbarContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options => {
            options.ConventionalControllers.Create(typeof(ManagementPortalApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "ManagementPortal API", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });
    }

    private void ConfigureExternalProviders(ServiceConfigurationContext context)
    {
        context.Services.AddAuthentication().AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
            options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "picture");
        }).WithDynamicOptions<GoogleOptions, GoogleHandler>(GoogleDefaults.AuthenticationScheme, options => {
            options.WithProperty(x => x.ClientId);
            options.WithProperty(x => x.ClientSecret, isSecret: true);
        }).AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options => {
            //Personal Microsoft accounts as an example.
            options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
            options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
            options.ClaimActions.MapCustomJson("picture", _ => "https://graph.microsoft.com/v1.0/me/photo/$value");
            options.SaveTokens = true;
        }).WithDynamicOptions<MicrosoftAccountOptions, MicrosoftAccountHandler>(MicrosoftAccountDefaults.AuthenticationScheme, options => {
            options.WithProperty(x => x.ClientId);
            options.WithProperty(x => x.ClientSecret, isSecret: true);
        }).AddTwitter(TwitterDefaults.AuthenticationScheme, options => {
            options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "profile_image_url_https");
            options.RetrieveUserDetails = true;
        }).WithDynamicOptions<TwitterOptions, TwitterHandler>(TwitterDefaults.AuthenticationScheme, options => {
            options.WithProperty(x => x.ConsumerKey);
            options.WithProperty(x => x.ConsumerSecret, isSecret: true);
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await base.OnApplicationInitializationAsync(context);
        var configService = context.ServiceProvider.GetRequiredService<DownloaderConfigService>();
        await configService.SeedFromJsonIfEmptyAsync();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        app.UseForwardedHeaders();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        app.UseAbpCookieConsent();
        app.UseCorrelationId();
        app.UseRouting();
        app.MapAbpStaticAssets();
        app.UseAbpStudioLink();
        app.UseAbpSecurityHeaders();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();
        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ManagementPortal API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
