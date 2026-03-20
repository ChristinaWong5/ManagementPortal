using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace ManagementPortal.DownloaderWebSockets;

public abstract class DownloaderWebSocketBase : FullAuditedEntity<Guid>
{
    public virtual Guid DownloaderId { get; set; }

    [CanBeNull]
    public virtual string? Host { get; set; }

    public virtual int Port { get; set; }

    protected DownloaderWebSocketBase()
    {
    }

    public DownloaderWebSocketBase(Guid id, Guid downloaderId, int port, string? host = null)
    {
        Id = id;
        DownloaderId = downloaderId;
        Port = port;
        Host = host;
    }
}
