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

namespace ManagementPortal.Downloaders;

public abstract class EfCoreDownloaderRepositoryBase : EfCoreRepository<ManagementPortalDbContext, Downloader, Guid>
{
    public EfCoreDownloaderRepositoryBase(IDbContextProvider<ManagementPortalDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, bool? downloaderEnabled = null, string? downloaderPollarName = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        query = ApplyFilter(query, filterText, downloaderEnabled, downloaderPollarName);
        var ids = query.Select(x => x.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Downloader>> GetListAsync(string? filterText = null, bool? downloaderEnabled = null, string? downloaderPollarName = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, downloaderEnabled, downloaderPollarName);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DownloaderConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, bool? downloaderEnabled = null, string? downloaderPollarName = null, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, downloaderEnabled, downloaderPollarName);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Downloader> ApplyFilter(IQueryable<Downloader> query, string? filterText = null, bool? downloaderEnabled = null, string? downloaderPollarName = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.DownloaderPollarName!.Contains(filterText!)).WhereIf(downloaderEnabled.HasValue, e => e.DownloaderEnabled == downloaderEnabled).WhereIf(!string.IsNullOrWhiteSpace(downloaderPollarName), e => e.DownloaderPollarName.Contains(downloaderPollarName));
    }
}
