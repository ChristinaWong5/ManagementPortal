using Volo.Abp.Application.Dtos;
using System;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderExcelDownloadDtoBase
{
    public string DownloadToken { get; set; } = null!;
    public string? FilterText { get; set; }

    public bool? DownloaderEnabled { get; set; }

    public string? DownloaderPollarName { get; set; }

    public DownloaderExcelDownloadDtoBase()
    {
    }
}
