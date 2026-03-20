using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using ManagementPortal.Downloaders;

namespace ManagementPortal.Downloaders;

public class DownloadersDataSeedContributor : IDataSeedContributor, ISingletonDependency
{
    private bool IsSeeded = false;
    private readonly IDownloaderRepository _downloaderRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public DownloadersDataSeedContributor(IDownloaderRepository downloaderRepository, IUnitOfWorkManager unitOfWorkManager)
    {
        _downloaderRepository = downloaderRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (IsSeeded)
        {
            return;
        }

        await _downloaderRepository.InsertAsync(new Downloader(id: Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045"), downloaderEnabled: true, downloaderPollarName: "c7fe11b1c8a74856abfc18578488961678968c7827514329b55a1b0bd7e5f1a4fe1263274d8c406388c33622dd99409f83"));
        await _downloaderRepository.InsertAsync(new Downloader(id: Guid.Parse("73995b0c-980f-4218-94a3-d143c8fb6649"), downloaderEnabled: true, downloaderPollarName: "0326309dcebb44f59599e3c3412f0dd38680312"));
        await _unitOfWorkManager!.Current!.SaveChangesAsync();
        IsSeeded = true;
    }
}
