using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementPortal.Downloaders;
using ManagementPortal.Web.Pages.Downloaders;

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
        try
        {
            DownloaderConfigs = await _configService.GetAllFromFileAsync();
            MaxWorker = await _configService.GetMaxWorkerAsync();
        }
        catch (Exception)
        {
            DownloaderConfigs = new List<DownloaderDto>();
        }
    }
}
