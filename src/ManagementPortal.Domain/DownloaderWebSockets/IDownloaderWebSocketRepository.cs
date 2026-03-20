using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ManagementPortal.DownloaderWebSockets;

public partial interface IDownloaderWebSocketRepository : IRepository<DownloaderWebSocket, Guid>
{
    Task<List<DownloaderWebSocket>> GetListByDownloaderIdAsync(Guid downloaderId, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountByDownloaderIdAsync(Guid downloaderId, CancellationToken cancellationToken = default);
    Task<List<DownloaderWebSocket>> GetListAsync(string? filterText = null, string? host = null, int? portMin = null, int? portMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? host = null, int? portMin = null, int? portMax = null, CancellationToken cancellationToken = default);
}
