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

public class EditModalModel : EditModalModelBase
{
    public EditModalModel(IDownloaderWebSocketsAppService downloaderWebSocketsAppService) : base(downloaderWebSocketsAppService)
    {
    }
}
