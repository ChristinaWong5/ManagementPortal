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

public class CreateModalModel : CreateModalModelBase
{
    public CreateModalModel(IDownloaderWebSocketsAppService downloaderWebSocketsAppService) : base(downloaderWebSocketsAppService)
    {
    }
}
