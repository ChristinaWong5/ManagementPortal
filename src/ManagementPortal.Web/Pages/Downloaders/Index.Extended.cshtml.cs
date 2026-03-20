using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using ManagementPortal.Downloaders;
using ManagementPortal.Shared;
using ManagementPortal.Web.Pages.Downloaders;

namespace ManagementPortal.Web.Pages.Downloaders;

public class IndexModel : IndexModelBase
{
    private readonly DownloaderConfigService _configService;

    public DownloaderDto? DownloaderConfig { get; set; }

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
        // Load config from JSON file
        try
        {
            DownloaderConfig = await _configService.GetFromFileAsync();
        }
        catch (Exception)
        {
            // file not found or invalid — config will be null
            DownloaderConfig = null;
        }
    }

    public async Task OnPostAsync(DownloaderDto input)
    {
        // Save back to JSON file
        await _configService.SaveToFileAsync(input);
    }
}
