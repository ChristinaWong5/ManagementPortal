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
using ManagementPortal.Downloaders;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using ManagementPortal.Shared;

namespace ManagementPortal.Downloaders;

[Authorize(ManagementPortalPermissions.Downloaders.Default)]
public abstract class DownloadersAppServiceBase : ManagementPortalAppService
{
    protected IDistributedCache<DownloaderDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IDownloaderRepository _downloaderRepository;
    protected DownloaderManager _downloaderManager;

    public DownloadersAppServiceBase(IDownloaderRepository downloaderRepository, DownloaderManager downloaderManager, IDistributedCache<DownloaderDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _downloaderRepository = downloaderRepository;
        _downloaderManager = downloaderManager;
    }

    public virtual async Task<PagedResultDto<DownloaderDto>> GetListAsync(GetDownloadersInput input)
    {
        var totalCount = await _downloaderRepository.GetCountAsync(input.FilterText, input.DownloaderEnabled, input.DownloaderPollarName);
        var items = await _downloaderRepository.GetListAsync(input.FilterText, input.DownloaderEnabled, input.DownloaderPollarName, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<DownloaderDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Downloader>, List<DownloaderDto>>(items)
        };
    }

    public virtual async Task<DownloaderDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Downloader, DownloaderDto>(await _downloaderRepository.GetAsync(id));
    }

    [Authorize(ManagementPortalPermissions.Downloaders.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _downloaderRepository.DeleteAsync(id);
    }

    [Authorize(ManagementPortalPermissions.Downloaders.Create)]
    public virtual async Task<DownloaderDto> CreateAsync(DownloaderCreateDto input)
    {
        var downloader = await _downloaderManager.CreateAsync(input.DownloaderEnabled, input.DownloaderPollarName);
        return ObjectMapper.Map<Downloader, DownloaderDto>(downloader);
    }

    [Authorize(ManagementPortalPermissions.Downloaders.Edit)]
    public virtual async Task<DownloaderDto> UpdateAsync(Guid id, DownloaderUpdateDto input)
    {
        var downloader = await _downloaderManager.UpdateAsync(id, input.DownloaderEnabled, input.DownloaderPollarName, input.ConcurrencyStamp);
        return ObjectMapper.Map<Downloader, DownloaderDto>(downloader);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DownloaderExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var items = await _downloaderRepository.GetListAsync(input.FilterText, input.DownloaderEnabled, input.DownloaderPollarName);
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Downloader>, List<DownloaderExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Downloaders.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(ManagementPortalPermissions.Downloaders.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> downloaderIds)
    {
        await _downloaderRepository.DeleteManyAsync(downloaderIds);
    }

    [Authorize(ManagementPortalPermissions.Downloaders.Delete)]
    public virtual async Task DeleteAllAsync(GetDownloadersInput input)
    {
        await _downloaderRepository.DeleteAllAsync(input.FilterText, input.DownloaderEnabled, input.DownloaderPollarName);
    }

    public virtual async Task<ManagementPortal.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new DownloaderDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new ManagementPortal.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}
