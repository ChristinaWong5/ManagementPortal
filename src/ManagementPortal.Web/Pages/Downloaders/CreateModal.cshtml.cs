using ManagementPortal.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ManagementPortal.Downloaders;

namespace ManagementPortal.Web.Pages.Downloaders;

public abstract class CreateModalModelBase : ManagementPortalPageModel
{
    [BindProperty]
    public DownloaderCreateViewModel Downloader { get; set; }

    protected IDownloadersAppService _downloadersAppService;

    public CreateModalModelBase(IDownloadersAppService downloadersAppService)
    {
        _downloadersAppService = downloadersAppService;
        Downloader = new();
    }

    public virtual async Task OnGetAsync()
    {
        Downloader = new DownloaderCreateViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _downloadersAppService.CreateAsync(ObjectMapper.Map<DownloaderCreateViewModel, DownloaderCreateDto>(Downloader));
        return NoContent();
    }
}

public class DownloaderCreateViewModel : DownloaderCreateDto
{
}
