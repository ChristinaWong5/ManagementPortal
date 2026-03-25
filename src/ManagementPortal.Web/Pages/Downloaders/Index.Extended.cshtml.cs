using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementPortal.Downloaders;

namespace ManagementPortal.Web.Pages.Downloaders;

public class IndexModel : IndexModelBase
{
    private readonly DownloaderConfigService _configService;

    public List<DownloaderDto> DownloaderConfigs { get; set; } = new();
    public int MaxWorker { get; set; }

    public IndexModel(
        IDownloadersAppService downloadersAppService,
        DownloaderConfigService configService)
        : base(downloadersAppService)
    {
        _configService = configService;
    }

    public override async Task OnGetAsync()
    {
        await base.OnGetAsync();
        var listTask = _downloadersAppService.GetListAsync(new GetDownloadersInput { MaxResultCount = 1000 });
        var maxWorkerTask = _configService.GetMaxWorkerAsync();
        await Task.WhenAll(listTask, maxWorkerTask);
        DownloaderConfigs = new List<DownloaderDto>(listTask.Result.Items);
        MaxWorker = maxWorkerTask.Result;
    }
}
