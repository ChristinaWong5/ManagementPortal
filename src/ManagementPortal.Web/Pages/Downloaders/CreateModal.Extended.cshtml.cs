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

public class CreateModalModel : CreateModalModelBase
{
    public CreateModalModel(IDownloadersAppService downloadersAppService) : base(downloadersAppService)
    {
    }
}
