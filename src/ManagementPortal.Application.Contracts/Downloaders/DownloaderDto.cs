using System;
using System.Collections.Generic;
using ManagementPortal.DownloaderWebSockets;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public bool DownloaderEnabled { get; set; }

    public string? DownloaderPollarName { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
    public List<DownloaderWebSocketDto> DownloaderWebSockets { get; set; } = new();
}
