using ManagementPortal.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace ManagementPortal.Permissions;

public class ManagementPortalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ManagementPortalPermissions.GroupName);
        myGroup.AddPermission(ManagementPortalPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ManagementPortalPermissions.MyPermission1, L("Permission:MyPermission1"));
        var downloaderPermission = myGroup.AddPermission(ManagementPortalPermissions.Downloaders.Default, L("Permission:Downloaders"));
        downloaderPermission.AddChild(ManagementPortalPermissions.Downloaders.Create, L("Permission:Create"));
        downloaderPermission.AddChild(ManagementPortalPermissions.Downloaders.Edit, L("Permission:Edit"));
        downloaderPermission.AddChild(ManagementPortalPermissions.Downloaders.Delete, L("Permission:Delete"));
        var downloaderWebSocketPermission = myGroup.AddPermission(ManagementPortalPermissions.DownloaderWebSockets.Default, L("Permission:DownloaderWebSockets"));
        downloaderWebSocketPermission.AddChild(ManagementPortalPermissions.DownloaderWebSockets.Create, L("Permission:Create"));
        downloaderWebSocketPermission.AddChild(ManagementPortalPermissions.DownloaderWebSockets.Edit, L("Permission:Edit"));
        downloaderWebSocketPermission.AddChild(ManagementPortalPermissions.DownloaderWebSockets.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ManagementPortalResource>(name);
    }
}
