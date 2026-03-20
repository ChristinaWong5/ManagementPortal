using Volo.Abp.Application.Dtos;
using System;

namespace ManagementPortal.DownloaderWebSockets;

public class GetDownloaderWebSocketListInput : PagedAndSortedResultRequestDto
{
    public Guid DownloaderId { get; set; }
}
