using Microsoft.AspNetCore.Builder;
using ManagementPortal;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ManagementPortal.Web.csproj"); 
await builder.RunAbpModuleAsync<ManagementPortalWebTestModule>(applicationName: "ManagementPortal.Web");

public partial class Program
{
}

