using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ManagementPortal.DownloaderWebSockets;

public abstract class DownloaderWebSocketCreateDtoBase
{
    public Guid DownloaderId { get; set; }

    public string? Host { get; set; }

    public int Port { get; set; }
}
