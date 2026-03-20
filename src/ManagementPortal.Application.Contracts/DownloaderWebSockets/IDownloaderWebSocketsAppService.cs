using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ManagementPortal.DownloaderWebSockets;

public partial interface IDownloaderWebSocketsAppService : IApplicationService
{
    Task<PagedResultDto<DownloaderWebSocketDto>> GetListByDownloaderIdAsync(GetDownloaderWebSocketListInput input);
    Task<PagedResultDto<DownloaderWebSocketDto>> GetListAsync(GetDownloaderWebSocketsInput input);
    Task<DownloaderWebSocketDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<DownloaderWebSocketDto> CreateAsync(DownloaderWebSocketCreateDto input);
    Task<DownloaderWebSocketDto> UpdateAsync(Guid id, DownloaderWebSocketUpdateDto input);
}
