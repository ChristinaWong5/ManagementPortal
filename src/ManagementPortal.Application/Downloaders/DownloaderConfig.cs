using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementPortal.Downloaders;

public class DownloaderConfig
{
    public string DownloaderEnabled { get; set; }
    public string DownloaderPollerName { get; set; }
    public List<DownloaderWebSocketConfig> DownloaderWebSocketList { get; set; }
}

public class DownloaderWebSocketConfig
{
    public int Port { get; set; }
    public string Host { get; set; }
}
