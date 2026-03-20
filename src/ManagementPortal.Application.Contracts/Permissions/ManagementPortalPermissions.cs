namespace ManagementPortal.Permissions;

public static class ManagementPortalPermissions
{
    public const string GroupName = "ManagementPortal";

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public static class Downloaders
    {
        public const string Default = GroupName + ".Downloaders";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class DownloaderWebSockets
    {
        public const string Default = GroupName + ".DownloaderWebSockets";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
