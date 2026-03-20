using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using ManagementPortal.EntityFrameworkCore;

namespace ManagementPortal.DownloaderWebSockets;

public abstract class EfCoreDownloaderWebSocketRepositoryBase : EfCoreRepository<ManagementPortalDbContext, DownloaderWebSocket, Guid>
{
    public EfCoreDownloaderWebSocketRepositoryBase(IDbContextProvider<ManagementPortalDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<DownloaderWebSocket>> GetListByDownloaderIdAsync(Guid downloaderId, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync()).Where(x => x.DownloaderId == downloaderId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DownloaderWebSocketConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountByDownloaderIdAsync(Guid downloaderId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.DownloaderId == downloaderId).CountAsync(cancellationToken);
    }

    public virtual async Task<List<DownloaderWebSocket>> GetListAsync(string? filterText = null, string? host = null, int? portMin = null, int? portMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, host, portMin, portMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DownloaderWebSocketConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? host = null, int? portMin = null, int? portMax = null, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, host, portMin, portMax);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<DownloaderWebSocket> ApplyFilter(IQueryable<DownloaderWebSocket> query, string? filterText = null, string? host = null, int? portMin = null, int? portMax = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Host!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(host), e => e.Host.Contains(host)).WhereIf(portMin.HasValue, e => e.Port >= portMin!.Value).WhereIf(portMax.HasValue, e => e.Port <= portMax!.Value);
    }
}
