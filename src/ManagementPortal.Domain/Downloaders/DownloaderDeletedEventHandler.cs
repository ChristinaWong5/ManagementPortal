using ManagementPortal.DownloaderWebSockets;
using ManagementPortal.Downloaders;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace ManagementPortal.Downloaders;

public class DownloaderDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<Downloader>>, ITransientDependency
{
    private readonly IDownloaderWebSocketRepository _downloaderWebSocketRepository;

    public DownloaderDeletedEventHandler(IDownloaderWebSocketRepository downloaderWebSocketRepository)
    {
        _downloaderWebSocketRepository = downloaderWebSocketRepository;
    }

    public async Task HandleEventAsync(EntityDeletedEventData<Downloader> eventData)
    {
        if (eventData.Entity is not ISoftDelete softDeletedEntity)
        {
            return;
        }

        if (!softDeletedEntity.IsDeleted)
        {
            return;
        }

        try
        {
            await _downloaderWebSocketRepository.DeleteManyAsync(await _downloaderWebSocketRepository.GetListByDownloaderIdAsync(eventData.Entity.Id));
        }
        catch
        {
            //...
        }
    }
}
