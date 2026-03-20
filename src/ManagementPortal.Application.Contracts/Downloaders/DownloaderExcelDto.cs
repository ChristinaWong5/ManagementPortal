using System;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderExcelDtoBase
{
    public bool DownloaderEnabled { get; set; }

    public string? DownloaderPollarName { get; set; }
}
