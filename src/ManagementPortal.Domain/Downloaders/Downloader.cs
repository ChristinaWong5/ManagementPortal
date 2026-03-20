using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using ManagementPortal.DownloaderWebSockets;
using Volo.Abp;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderBase : FullAuditedAggregateRoot<Guid>
{
    public virtual bool DownloaderEnabled { get; set; }

    [CanBeNull]
    public virtual string? DownloaderPollarName { get; set; }

    public ICollection<DownloaderWebSocket> DownloaderWebSockets { get; protected set; } = new Collection<DownloaderWebSocket>();

    protected DownloaderBase()
    {
    }

    public DownloaderBase(Guid id, bool downloaderEnabled, string? downloaderPollarName = null)
    {
        Id = id;
        DownloaderEnabled = downloaderEnabled;
        DownloaderPollarName = downloaderPollarName;
    }
}
