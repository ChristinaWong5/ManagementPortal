using ManagementPortal.DownloaderWebSockets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderUpdateDtoBase : IHasConcurrencyStamp
{
    public bool DownloaderEnabled { get; set; }

    public string? DownloaderPollarName { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
    public List<DownloaderWebSocketDto> DownloaderWebSockets { get; set; } = new();

}
