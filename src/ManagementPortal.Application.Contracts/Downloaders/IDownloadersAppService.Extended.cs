using System.Threading.Tasks;

namespace ManagementPortal.Downloaders;

public partial interface IDownloadersAppService
{
    Task<int> GetMaxWorkerAsync();
    Task SetMaxWorkerAsync(int maxWorker);
}
