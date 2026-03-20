using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using ManagementPortal.Shared;

namespace ManagementPortal.Downloaders;

public partial interface IDownloadersAppService : IApplicationService
{
    Task<PagedResultDto<DownloaderDto>> GetListAsync(GetDownloadersInput input);
    Task<DownloaderDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<DownloaderDto> CreateAsync(DownloaderCreateDto input);
    Task<DownloaderDto> UpdateAsync(Guid id, DownloaderUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(DownloaderExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> downloaderIds);
    Task DeleteAllAsync(GetDownloadersInput input);
    Task<ManagementPortal.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}
