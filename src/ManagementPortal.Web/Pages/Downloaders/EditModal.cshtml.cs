using ManagementPortal.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using ManagementPortal.Downloaders;

namespace ManagementPortal.Web.Pages.Downloaders;

public abstract class EditModalModelBase : ManagementPortalPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public DownloaderUpdateViewModel Downloader { get; set; }

    protected IDownloadersAppService _downloadersAppService;

    public EditModalModelBase(IDownloadersAppService downloadersAppService)
    {
        _downloadersAppService = downloadersAppService;
        Downloader = new();
    }

    public virtual async Task OnGetAsync()
    {
        var downloader = await _downloadersAppService.GetAsync(Id);
        Downloader = ObjectMapper.Map<DownloaderDto, DownloaderUpdateViewModel>(downloader);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _downloadersAppService.UpdateAsync(Id, ObjectMapper.Map<DownloaderUpdateViewModel, DownloaderUpdateDto>(Downloader));
        return NoContent();
    }
}

public class DownloaderUpdateViewModel : DownloaderUpdateDto
{
}
