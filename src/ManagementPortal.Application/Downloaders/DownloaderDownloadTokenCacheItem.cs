using System;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}
