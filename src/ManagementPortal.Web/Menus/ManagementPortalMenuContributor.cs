using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ManagementPortal.Localization;
using ManagementPortal.Permissions;
using ManagementPortal.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.AuditLogging.Web.Navigation;
using Volo.Abp.LanguageManagement.Navigation;
using Volo.FileManagement.Web.Navigation;
using Volo.Abp.TextTemplateManagement.Web.Navigation;
using Volo.Abp.OpenIddict.Pro.Web.Menus;

namespace ManagementPortal.Web.Menus;

public class ManagementPortalMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<ManagementPortalResource>();
        //Home
        context.Menu.AddItem(new ApplicationMenuItem(ManagementPortalMenus.Home, l["Menu:Home"], "~/", icon: "fa fa-home", order: 1));
        //HostDashboard
        context.Menu.AddItem(new ApplicationMenuItem(ManagementPortalMenus.HostDashboard, l["Menu:Dashboard"], "~/HostDashboard", icon: "fa fa-line-chart", order: 2).RequirePermissions(ManagementPortalPermissions.Dashboard.Host));
        //File management
        context.Menu.SetSubItemOrder(FileManagementMenuNames.GroupName, 5);

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;
        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        //Administration->OpenIddict
        administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 3);
        //Administration->Language Management
        administration.SetSubItemOrder(LanguageManagementMenuNames.GroupName, 4);
        //Administration->Text Template Management
        administration.SetSubItemOrder(TextTemplateManagementMainMenuNames.GroupName, 6);
        //Administration->Audit Logs
        administration.SetSubItemOrder(AbpAuditLoggingMainMenuNames.GroupName, 7);
        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 8);
        context.Menu
            //.AddItem(
            //new ApplicationMenuItem(
            //    "SFTP Bridge",
            //    "SFTP Bridge"
            //)
            .AddItem(
                    new ApplicationMenuItem(ManagementPortalMenus.Downloaders, 
                    l["Menu:Downloaders"], 
                    url: "/Downloaders", 
                    icon: "fa fa-file-alt", 
                    requiredPermissionName: ManagementPortalPermissions.Downloaders.Default)
                //)
            );
        return Task.CompletedTask;
    }
}
