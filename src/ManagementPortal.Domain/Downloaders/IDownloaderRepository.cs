using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ManagementPortal.Downloaders;

public partial interface IDownloaderRepository : IRepository<Downloader, Guid>
{
    Task DeleteAllAsync(string? filterText = null, bool? downloaderEnabled = null, string? downloaderPollarName = null, CancellationToken cancellationToken = default);
    Task<List<Downloader>> GetListAsync(string? filterText = null, bool? downloaderEnabled = null, string? downloaderPollarName = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, bool? downloaderEnabled = null, string? downloaderPollarName = null, CancellationToken cancellationToken = default);
}
