using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ManagementPortal.DownloaderWebSockets;

public abstract class DownloaderWebSocketDtoBase : FullAuditedEntityDto<Guid>
{
    public Guid DownloaderId { get; set; }

    public string? Host { get; set; }

    public int Port { get; set; }
}
