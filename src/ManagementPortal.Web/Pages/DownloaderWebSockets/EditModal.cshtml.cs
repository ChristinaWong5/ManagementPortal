using ManagementPortal.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using ManagementPortal.DownloaderWebSockets;

namespace ManagementPortal.Web.Pages.DownloaderWebSockets;

public abstract class EditModalModelBase : ManagementPortalPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public DownloaderWebSocketUpdateViewModel DownloaderWebSocket { get; set; }

    protected IDownloaderWebSocketsAppService _downloaderWebSocketsAppService;

    public EditModalModelBase(IDownloaderWebSocketsAppService downloaderWebSocketsAppService)
    {
        _downloaderWebSocketsAppService = downloaderWebSocketsAppService;
        DownloaderWebSocket = new();
    }

    public virtual async Task OnGetAsync()
    {
        var downloaderWebSocket = await _downloaderWebSocketsAppService.GetAsync(Id);
        DownloaderWebSocket = ObjectMapper.Map<DownloaderWebSocketDto, DownloaderWebSocketUpdateViewModel>(downloaderWebSocket);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _downloaderWebSocketsAppService.UpdateAsync(Id, ObjectMapper.Map<DownloaderWebSocketUpdateViewModel, DownloaderWebSocketUpdateDto>(DownloaderWebSocket));
        return NoContent();
    }
}

public class DownloaderWebSocketUpdateViewModel : DownloaderWebSocketUpdateDto
{
}
