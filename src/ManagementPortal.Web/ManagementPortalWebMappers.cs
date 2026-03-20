using ManagementPortal.DownloaderWebSockets;
using ManagementPortal.Web.Pages.DownloaderWebSockets;
using ManagementPortal.Downloaders;
using ManagementPortal.Web.Pages.Downloaders;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace ManagementPortal.Web;

/*
 * You can add your own mappings here.
 * [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
 * public partial class ManagementPortalWebMappers : MapperBase<BookDto, CreateUpdateBookDto>
 * {
 *    public override partial CreateUpdateBookDto Map(BookDto source);
 * 
 *    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
 * }
 */
[Mapper]
public partial class DownloaderDtoToDownloaderUpdateViewModelMapper : MapperBase<DownloaderDto, DownloaderUpdateViewModel>
{
    public override partial DownloaderUpdateViewModel Map(DownloaderDto source);
    public override partial void Map(DownloaderDto source, DownloaderUpdateViewModel destination);
}

[Mapper]
public partial class DownloaderUpdateViewModelToDownloaderUpdateDto : MapperBase<DownloaderUpdateViewModel, DownloaderUpdateDto>
{
    public override partial DownloaderUpdateDto Map(DownloaderUpdateViewModel source);
    public override partial void Map(DownloaderUpdateViewModel source, DownloaderUpdateDto destination);
}

[Mapper]
public partial class DownloaderCreateViewModelToDownloaderCreateDto : MapperBase<DownloaderCreateViewModel, DownloaderCreateDto>
{
    public override partial DownloaderCreateDto Map(DownloaderCreateViewModel source);
    public override partial void Map(DownloaderCreateViewModel source, DownloaderCreateDto destination);
}

[Mapper]
public partial class DownloaderWebSocketDtoToDownloaderWebSocketUpdateViewModelMapper : MapperBase<DownloaderWebSocketDto, DownloaderWebSocketUpdateViewModel>
{
    public override partial DownloaderWebSocketUpdateViewModel Map(DownloaderWebSocketDto source);
    public override partial void Map(DownloaderWebSocketDto source, DownloaderWebSocketUpdateViewModel destination);
}

[Mapper]
public partial class DownloaderWebSocketUpdateViewModelToDownloaderWebSocketUpdateDto : MapperBase<DownloaderWebSocketUpdateViewModel, DownloaderWebSocketUpdateDto>
{
    public override partial DownloaderWebSocketUpdateDto Map(DownloaderWebSocketUpdateViewModel source);
    public override partial void Map(DownloaderWebSocketUpdateViewModel source, DownloaderWebSocketUpdateDto destination);
}

[Mapper]
public partial class DownloaderWebSocketCreateViewModelToDownloaderWebSocketCreateDto : MapperBase<DownloaderWebSocketCreateViewModel, DownloaderWebSocketCreateDto>
{
    public override partial DownloaderWebSocketCreateDto Map(DownloaderWebSocketCreateViewModel source);
    public override partial void Map(DownloaderWebSocketCreateViewModel source, DownloaderWebSocketCreateDto destination);
}
