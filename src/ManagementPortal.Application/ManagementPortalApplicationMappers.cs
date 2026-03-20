using ManagementPortal.DownloaderWebSockets;
using ManagementPortal.Downloaders;
using System;
using ManagementPortal.Shared;
using System.Linq;
using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace ManagementPortal;

/*
 * You can add your own mappings here.
 * [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
 * public partial class ManagementPortalApplicationMappers : MapperBase<BookDto, CreateUpdateBookDto>
 * {
 *    public override partial CreateUpdateBookDto Map(BookDto source);
 * 
 *    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
 * }
 */
[Mapper]
public partial class DownloaderToDownloaderDtoMappers : MapperBase<Downloader, DownloaderDto>
{
    [MapperIgnoreTarget(nameof(DownloaderDto.DownloaderWebSockets))]
    public override partial DownloaderDto Map(Downloader source);
    [MapperIgnoreTarget(nameof(DownloaderDto.DownloaderWebSockets))]
    public override partial void Map(Downloader source, DownloaderDto destination);
}

[Mapper]
public partial class DownloaderToDownloaderExcelDtoMappers : MapperBase<Downloader, DownloaderExcelDto>
{
    public override partial DownloaderExcelDto Map(Downloader source);
    public override partial void Map(Downloader source, DownloaderExcelDto destination);
}

[Mapper]
public partial class DownloaderWebSocketToDownloaderWebSocketDtoMappers : MapperBase<DownloaderWebSocket, DownloaderWebSocketDto>
{
    public override partial DownloaderWebSocketDto Map(DownloaderWebSocket source);
    public override partial void Map(DownloaderWebSocket source, DownloaderWebSocketDto destination);
}
