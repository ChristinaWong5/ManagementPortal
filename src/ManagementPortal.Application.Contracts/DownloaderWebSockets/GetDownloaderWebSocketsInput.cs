using Volo.Abp.Application.Dtos;
using System;

namespace ManagementPortal.DownloaderWebSockets;

public abstract class GetDownloaderWebSocketsInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Host { get; set; }

    public int? PortMin { get; set; }

    public int? PortMax { get; set; }

    public GetDownloaderWebSocketsInputBase()
    {
    }
}
