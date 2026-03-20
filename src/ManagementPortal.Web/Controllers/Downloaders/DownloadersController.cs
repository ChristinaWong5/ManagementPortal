using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace ManagementPortal.Web.Controllers.Downloaders;

[Route("[controller]/[action]")]
public class DownloadersController : AbpController
{
    [HttpGet]
    public virtual async Task<PartialViewResult> ChildDataGrid(Guid downloaderId)
    {
        return PartialView("~/Pages/Shared/Downloaders/_ChildDataGrids.cshtml", downloaderId);
    }
}
