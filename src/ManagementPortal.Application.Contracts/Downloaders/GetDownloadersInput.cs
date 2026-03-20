using Volo.Abp.Application.Dtos;
using System;

namespace ManagementPortal.Downloaders;

public abstract class GetDownloadersInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public bool? DownloaderEnabled { get; set; }

    public string? DownloaderPollarName { get; set; }

    public GetDownloadersInputBase()
    {
    }
}
