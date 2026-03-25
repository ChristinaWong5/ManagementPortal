using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace ManagementPortal.Downloaders;

public class DownloaderJsonSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly DownloaderConfigService _configService;

    public DownloaderJsonSeedContributor(DownloaderConfigService configService)
    {
        _configService = configService;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _configService.SeedFromJsonIfEmptyAsync();
    }
}