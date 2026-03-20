using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using ManagementPortal.Permissions;
using ManagementPortal.DownloaderWebSockets;

namespace ManagementPortal.DownloaderWebSockets;

[Authorize(ManagementPortalPermissions.DownloaderWebSockets.Default)]
public abstract class DownloaderWebSocketsAppServiceBase : ManagementPortalAppService
{
    protected IDownloaderWebSocketRepository _downloaderWebSocketRepository;
    protected DownloaderWebSocketManager _downloaderWebSocketManager;

    public DownloaderWebSocketsAppServiceBase(IDownloaderWebSocketRepository downloaderWebSocketRepository, DownloaderWebSocketManager downloaderWebSocketManager)
    {
        _downloaderWebSocketRepository = downloaderWebSocketRepository;
        _downloaderWebSocketManager = downloaderWebSocketManager;
    }

    public virtual async Task<PagedResultDto<DownloaderWebSocketDto>> GetListByDownloaderIdAsync(GetDownloaderWebSocketListInput input)
    {
        var downloaderWebSockets = await _downloaderWebSocketRepository.GetListByDownloaderIdAsync(input.DownloaderId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<DownloaderWebSocketDto>
        {
            TotalCount = await _downloaderWebSocketRepository.GetCountByDownloaderIdAsync(input.DownloaderId),
            Items = ObjectMapper.Map<List<DownloaderWebSocket>, List<DownloaderWebSocketDto>>(downloaderWebSockets)
        };
    }

    public virtual async Task<PagedResultDto<DownloaderWebSocketDto>> GetListAsync(GetDownloaderWebSocketsInput input)
    {
        var totalCount = await _downloaderWebSocketRepository.GetCountAsync(input.FilterText, input.Host, input.PortMin, input.PortMax);
        var items = await _downloaderWebSocketRepository.GetListAsync(input.FilterText, input.Host, input.PortMin, input.PortMax, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<DownloaderWebSocketDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<DownloaderWebSocket>, List<DownloaderWebSocketDto>>(items)
        };
    }

    public virtual async Task<DownloaderWebSocketDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<DownloaderWebSocket, DownloaderWebSocketDto>(await _downloaderWebSocketRepository.GetAsync(id));
    }

    [Authorize(ManagementPortalPermissions.DownloaderWebSockets.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _downloaderWebSocketRepository.DeleteAsync(id);
    }

    [Authorize(ManagementPortalPermissions.DownloaderWebSockets.Create)]
    public virtual async Task<DownloaderWebSocketDto> CreateAsync(DownloaderWebSocketCreateDto input)
    {
        var downloaderWebSocket = await _downloaderWebSocketManager.CreateAsync(input.DownloaderId, input.Port, input.Host);
        return ObjectMapper.Map<DownloaderWebSocket, DownloaderWebSocketDto>(downloaderWebSocket);
    }

    [Authorize(ManagementPortalPermissions.DownloaderWebSockets.Edit)]
    public virtual async Task<DownloaderWebSocketDto> UpdateAsync(Guid id, DownloaderWebSocketUpdateDto input)
    {
        var downloaderWebSocket = await _downloaderWebSocketManager.UpdateAsync(id, input.DownloaderId, input.Port, input.Host);
        return ObjectMapper.Map<DownloaderWebSocket, DownloaderWebSocketDto>(downloaderWebSocket);
    }
}
