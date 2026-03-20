using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ManagementPortal.Downloaders;

public abstract class DownloaderCreateDtoBase
{
    public bool DownloaderEnabled { get; set; }

    public string? DownloaderPollarName { get; set; }
}
