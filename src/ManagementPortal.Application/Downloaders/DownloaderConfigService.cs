using ManagementPortal.DownloaderWebSockets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ManagementPortal.Downloaders;

public class DownloaderConfigService : ManagementPortalAppService
{
    private readonly string _configFilePath;

    public DownloaderConfigService(IConfiguration configuration)
    {
        // path to your JSON config file
        _configFilePath = configuration["DownloaderConfigFilePath"]
                          ?? "C:\\Users\\KioskAdmin\\Documents\\myFiles\\Test\\config.json";
    }

    // READ - load from JSON file
    public async Task<DownloaderDto> GetFromFileAsync()
    {
        var json = await File.ReadAllTextAsync(_configFilePath);
        var config = JsonSerializer.Deserialize<DownloaderConfig>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return new DownloaderDto
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ConcurrencyStamp = "00000000-0000-0000-0000-000000000001",
            DownloaderEnabled = bool.Parse(config.DownloaderEnabled),
            DownloaderPollarName = config.DownloaderPollerName,
            DownloaderWebSockets = config.DownloaderWebSocketList
                .Select((x, index) => new DownloaderWebSocketDto
                {
                    Id = Guid.Parse($"00000000-0000-0000-0000-{(index + 1):D12}"),
                    DownloaderId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Host = x.Host,
                    Port = x.Port
                }).ToList()
        };
    }

    // SAVE - write back to JSON file
    public async Task SaveToFileAsync(DownloaderDto input)
    {
        var config = new DownloaderConfig
        {
            DownloaderEnabled = input.DownloaderEnabled.ToString().ToLower(),
            DownloaderPollerName = input.DownloaderPollarName,
            DownloaderWebSocketList = input.DownloaderWebSockets
                .Select(x => new DownloaderWebSocketConfig
                {
                    Host = x.Host,
                    Port = x.Port
                }).ToList()
        };

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await File.WriteAllTextAsync(_configFilePath, json);
    }

    public async Task<PagedResultDto<DownloaderDto>> GetPagedListFromFileAsync(
    string filterText = null,
    string downloaderPollarName = null,
    bool? downloaderEnabled = null)
    {
        var dto = await GetFromFileAsync();

        // Put single config into a list
        var list = new List<DownloaderDto> { dto };

        // Apply filters
        if (!filterText.IsNullOrWhiteSpace())
        {
            list = list.Where(x =>
                x.DownloaderPollarName != null &&
                x.DownloaderPollarName.Contains(filterText,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (downloaderEnabled.HasValue)
        {
            list = list.Where(x => x.DownloaderEnabled == downloaderEnabled.Value)
                .ToList();
        }

        return new PagedResultDto<DownloaderDto>(list.Count, list);
    }
}

