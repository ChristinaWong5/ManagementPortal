using ManagementPortal.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ManagementPortal.DownloaderWebSockets;

namespace ManagementPortal.Web.Pages.DownloaderWebSockets;

public abstract class CreateModalModelBase : ManagementPortalPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid DownloaderId { get; set; }

    [BindProperty]
    public DownloaderWebSocketCreateViewModel DownloaderWebSocket { get; set; }

    protected IDownloaderWebSocketsAppService _downloaderWebSocketsAppService;

    public CreateModalModelBase(IDownloaderWebSocketsAppService downloaderWebSocketsAppService)
    {
        _downloaderWebSocketsAppService = downloaderWebSocketsAppService;
        DownloaderWebSocket = new();
    }

    public virtual async Task OnGetAsync()
    {
        DownloaderWebSocket = new DownloaderWebSocketCreateViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        DownloaderWebSocket.DownloaderId = DownloaderId;
        await _downloaderWebSocketsAppService.CreateAsync(ObjectMapper.Map<DownloaderWebSocketCreateViewModel, DownloaderWebSocketCreateDto>(DownloaderWebSocket));
        return NoContent();
    }
}

public class DownloaderWebSocketCreateViewModel : DownloaderWebSocketCreateDto
{
}
