namespace ManagementPortal.DownloaderWebSockets;

public static class DownloaderWebSocketConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "DownloaderWebSocket." : string.Empty);
    }
}
