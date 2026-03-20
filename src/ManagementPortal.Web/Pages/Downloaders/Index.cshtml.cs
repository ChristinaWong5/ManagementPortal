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

namespace ManagementPortal.Web.Pages.Downloaders;

public abstract class IndexModelBase : AbpPageModel
{
    [SelectItems(nameof(DownloaderEnabledBoolFilterItems))]
    public string DownloaderEnabledFilter { get; set; }

    public List<SelectListItem> DownloaderEnabledBoolFilterItems { get; set; } = new List<SelectListItem> { new SelectListItem("", ""),
        new SelectListItem("Yes", "true"),
        new SelectListItem("No", "false"),
    };
    public string? DownloaderPollarNameFilter { get; set; }

    protected IDownloadersAppService _downloadersAppService;

    public IndexModelBase(IDownloadersAppService downloadersAppService)
    {
        _downloadersAppService = downloadersAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}
