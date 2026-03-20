using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ManagementPortal.DownloaderWebSockets;

public abstract class DownloaderWebSocketManagerBase : DomainService
{
    protected IDownloaderWebSocketRepository _downloaderWebSocketRepository;

    public DownloaderWebSocketManagerBase(IDownloaderWebSocketRepository downloaderWebSocketRepository)
    {
        _downloaderWebSocketRepository = downloaderWebSocketRepository;
    }

    public virtual async Task<DownloaderWebSocket> CreateAsync(Guid downloaderId, int port, string? host = null)
    {
        var downloaderWebSocket = new DownloaderWebSocket(GuidGenerator.Create(), downloaderId, port, host);
        return await _downloaderWebSocketRepository.InsertAsync(downloaderWebSocket);
    }

    public virtual async Task<DownloaderWebSocket> UpdateAsync(Guid id, Guid downloaderId, int port, string? host = null)
    {
        var downloaderWebSocket = await _downloaderWebSocketRepository.GetAsync(id);
        downloaderWebSocket.DownloaderId = downloaderId;
        downloaderWebSocket.Port = port;
        downloaderWebSocket.Host = host;
        return await _downloaderWebSocketRepository.UpdateAsync(downloaderWebSocket);
    }
}
