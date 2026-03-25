using ManagementPortal.DownloaderWebSockets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.SettingManagement;
using Volo.Abp.Uow;

namespace ManagementPortal.Downloaders;

public class DownloaderConfigService : ITransientDependency
{
    private readonly string _configFilePath;
    private readonly IDownloaderRepository _downloaderRepository;
    private readonly IRepository<DownloaderWebSocket, Guid> _wsRepository;
    private readonly ISettingManager _settingManager;

    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true
    };

    public DownloaderConfigService(
        IConfiguration configuration,
        IDownloaderRepository downloaderRepository,
        IRepository<DownloaderWebSocket, Guid> wsRepository,
        ISettingManager settingManager)
    {
        _configFilePath = configuration["DownloaderConfigFilePath"]
                          ?? "C:\\Users\\KioskAdmin\\Documents\\myFiles\\Test\\config.json";
        _downloaderRepository = downloaderRepository;
        _wsRepository = wsRepository;
        _settingManager = settingManager;
    }

    // --- MaxWorker (stored in DB via ABP Settings) ---

    public async Task<int> GetMaxWorkerAsync()
    {
        var value = await _settingManager.GetOrNullGlobalAsync(DownloaderSettings.MaxWorker);
        if (int.TryParse(value, out var result))
            return result;

        // Not in DB yet — read from config.json and store it for next time
        var root = await ReadRootAsync();
        await _settingManager.SetGlobalAsync(DownloaderSettings.MaxWorker, root.MaxWorker.ToString());
        return root.MaxWorker;
    }

    public async Task SetMaxWorkerAsync(int maxWorker)
    {
        await _settingManager.SetGlobalAsync(DownloaderSettings.MaxWorker, maxWorker.ToString());
        await ExportToJsonAsync();
    }

    // --- Export DB → JSON ---

    public async Task ExportToJsonAsync()
    {
        var maxWorker = await GetMaxWorkerAsync();
        var downloadersTask = _downloaderRepository.GetListAsync();
        var wsTask = _wsRepository.GetListAsync();
        await Task.WhenAll(downloadersTask, wsTask);
        var downloaders = downloadersTask.Result;
        var allWebSockets = wsTask.Result;

        var wsLookup = allWebSockets
            .GroupBy(w => w.DownloaderId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var root = new DownloaderRootConfig
        {
            MaxWorker = maxWorker,
            DownloaderConfig = downloaders.Select(d => new DownloaderItemConfig
            {
                Enabled = d.DownloaderEnabled,
                Name = d.DownloaderPollarName ?? string.Empty,
                DownstreamHealthFile = d.DownstreamHealthFile ?? string.Empty,
                WebsocketConfig = wsLookup.TryGetValue(d.Id, out var wsList)
                    ? wsList.Select(w => new WebSocketItemConfig { Host = w.Host ?? string.Empty, Port = w.Port }).ToList()
                    : new List<WebSocketItemConfig>()
            }).ToList()
        };

        await WriteRootAsync(root);
    }

    // --- Seed DB from JSON (runs once if DB is empty) ---

    [UnitOfWork]
    public virtual async Task SeedFromJsonIfEmptyAsync()
    {
        var count = await _downloaderRepository.GetCountAsync();
        if (count > 0)
            return;

        var root = await ReadRootAsync();
        await _settingManager.SetGlobalAsync(DownloaderSettings.MaxWorker, root.MaxWorker.ToString());

        foreach (var item in root.DownloaderConfig)
        {
            var downloader = new Downloader(Guid.NewGuid(), item.Enabled, item.Name)
            {
                DownstreamHealthFile = item.DownstreamHealthFile
            };
            downloader = await _downloaderRepository.InsertAsync(downloader);

            foreach (var ws in item.WebsocketConfig)
            {
                var webSocket = new DownloaderWebSocket(Guid.NewGuid(), downloader.Id, ws.Port, ws.Host);
                await _wsRepository.InsertAsync(webSocket);
            }
        }
    }

    // --- File I/O ---

    private async Task<DownloaderRootConfig> ReadRootAsync()
    {
        var json = await File.ReadAllTextAsync(_configFilePath);
        return JsonSerializer.Deserialize<DownloaderRootConfig>(json, ReadOptions)
               ?? new DownloaderRootConfig();
    }

    private async Task WriteRootAsync(DownloaderRootConfig root)
    {
        var json = JsonSerializer.Serialize(root, WriteOptions);
        await File.WriteAllTextAsync(_configFilePath, json);
    }
}