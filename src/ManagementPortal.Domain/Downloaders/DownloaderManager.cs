using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderManagerBase : DomainService
{
    protected IDownloaderRepository _downloaderRepository;

    public DownloaderManagerBase(IDownloaderRepository downloaderRepository)
    {
        _downloaderRepository = downloaderRepository;
    }

    public virtual async Task<Downloader> CreateAsync(bool downloaderEnabled, string? downloaderPollarName = null)
    {
        var downloader = new Downloader(GuidGenerator.Create(), downloaderEnabled, downloaderPollarName);
        return await _downloaderRepository.InsertAsync(downloader);
    }

    public virtual async Task<Downloader> UpdateAsync(Guid id, bool downloaderEnabled, string? downloaderPollarName = null, [CanBeNull] string? concurrencyStamp = null)
    {
        var downloader = await _downloaderRepository.GetAsync(id);
        downloader.DownloaderEnabled = downloaderEnabled;
        downloader.DownloaderPollarName = downloaderPollarName;
        downloader.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _downloaderRepository.UpdateAsync(downloader);
    }
}
