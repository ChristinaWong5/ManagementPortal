using ManagementPortal.DownloaderWebSockets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ManagementPortal.Downloaders;

public class DownloaderConfigService : ManagementPortalAppService
{
    private readonly string _configFilePath;

    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true
    };

    public DownloaderConfigService(IConfiguration configuration)
    {
        _configFilePath = configuration["DownloaderConfigFilePath"]
                          ?? "C:\\Users\\KioskAdmin\\Documents\\myFiles\\Test\\config.json";
    }

    // --- Downloader ID helpers ---

    private static Guid IndexToId(int index) =>
        Guid.Parse($"00000000-0000-0000-0000-{(index + 1):X12}");

    private static int IdToIndex(Guid id)
    {
        var hex = id.ToString("N").Substring(20); // last 12 hex chars
        return int.Parse(hex, NumberStyles.HexNumber) - 1;
    }

    // --- WebSocket ID helpers ---
    // Format: 00000000-0000-{dlIndex+1 as X4}-0000-{wsIndex+1 as X12}

    private static Guid WsIndexToId(int dlIndex, int wsIndex) =>
        Guid.Parse($"00000000-0000-{(dlIndex + 1):X4}-0000-{(wsIndex + 1):X12}");

    private static (int dlIndex, int wsIndex) ParseWsId(Guid wsId)
    {
        var s = wsId.ToString("N"); // 32 hex chars, no dashes
        var dlPart = s.Substring(12, 4);
        var wsPart = s.Substring(20, 12);
        return (int.Parse(dlPart, NumberStyles.HexNumber) - 1,
                int.Parse(wsPart, NumberStyles.HexNumber) - 1);
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

    // --- Downloader mapping ---

    private static DownloaderDto MapToDto(DownloaderItemConfig item, int dlIndex) => new()
    {
        Id = IndexToId(dlIndex),
        ConcurrencyStamp = IndexToId(dlIndex).ToString(),
        DownloaderEnabled = item.Enabled,
        DownloaderPollarName = item.Name,
        DownstreamHealthFile = item.DownstreamHealthFile,
        DownloaderWebSockets = item.WebsocketConfig
            .Select((ws, wsIdx) => new DownloaderWebSocketDto
            {
                Id = WsIndexToId(dlIndex, wsIdx),
                DownloaderId = IndexToId(dlIndex),
                Host = ws.Host,
                Port = ws.Port
            }).ToList()
    };

    private static DownloaderItemConfig MapToConfig(DownloaderDto dto) => new()
    {
        Enabled = dto.DownloaderEnabled,
        Name = dto.DownloaderPollarName ?? string.Empty,
        DownstreamHealthFile = dto.DownstreamHealthFile ?? string.Empty,
        WebsocketConfig = dto.DownloaderWebSockets
            .Select(ws => new WebSocketItemConfig { Host = ws.Host ?? string.Empty, Port = ws.Port })
            .ToList()
    };

    // --- Downloader CRUD ---

    public async Task<List<DownloaderDto>> GetAllFromFileAsync()
    {
        var root = await ReadRootAsync();
        return root.DownloaderConfig
            .Select((item, index) => MapToDto(item, index))
            .ToList();
    }

    public async Task<DownloaderDto?> GetFromFileAsync(Guid id)
    {
        var root = await ReadRootAsync();
        var index = IdToIndex(id);
        if (index < 0 || index >= root.DownloaderConfig.Count)
            return null;
        return MapToDto(root.DownloaderConfig[index], index);
    }

    public async Task<DownloaderDto> AddToFileAsync(DownloaderDto input)
    {
        var root = await ReadRootAsync();
        root.DownloaderConfig.Add(MapToConfig(input));
        await WriteRootAsync(root);
        var newIndex = root.DownloaderConfig.Count - 1;
        return MapToDto(root.DownloaderConfig[newIndex], newIndex);
    }

    public async Task<DownloaderDto> UpdateInFileAsync(Guid id, DownloaderDto input)
    {
        var root = await ReadRootAsync();
        var index = IdToIndex(id);
        if (index < 0 || index >= root.DownloaderConfig.Count)
            throw new InvalidOperationException($"Downloader with id {id} not found.");
        root.DownloaderConfig[index] = MapToConfig(input);
        await WriteRootAsync(root);
        return MapToDto(root.DownloaderConfig[index], index);
    }

    public async Task DeleteFromFileAsync(Guid id)
    {
        var root = await ReadRootAsync();
        var index = IdToIndex(id);
        if (index < 0 || index >= root.DownloaderConfig.Count)
            throw new InvalidOperationException($"Downloader with id {id} not found.");
        root.DownloaderConfig.RemoveAt(index);
        await WriteRootAsync(root);
    }

    public async Task<PagedResultDto<DownloaderDto>> GetPagedListFromFileAsync(
        string filterText = null,
        string downloaderPollarName = null,
        bool? downloaderEnabled = null)
    {
        var list = await GetAllFromFileAsync();

        if (!filterText.IsNullOrWhiteSpace())
            list = list.Where(x => x.DownloaderPollarName != null &&
                x.DownloaderPollarName.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!downloaderPollarName.IsNullOrWhiteSpace())
            list = list.Where(x => x.DownloaderPollarName != null &&
                x.DownloaderPollarName.Contains(downloaderPollarName, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (downloaderEnabled.HasValue)
            list = list.Where(x => x.DownloaderEnabled == downloaderEnabled.Value).ToList();

        return new PagedResultDto<DownloaderDto>(list.Count, list);
    }

    // --- MaxWorker ---

    public async Task<int> GetMaxWorkerAsync()
    {
        var root = await ReadRootAsync();
        return root.MaxWorker;
    }

    public async Task SetMaxWorkerAsync(int maxWorker)
    {
        var root = await ReadRootAsync();
        root.MaxWorker = maxWorker;
        await WriteRootAsync(root);
    }

    // --- WebSocket CRUD ---

    public async Task<DownloaderWebSocketDto?> GetWebSocketAsync(Guid wsId)
    {
        var (dlIndex, wsIndex) = ParseWsId(wsId);
        var root = await ReadRootAsync();
        if (dlIndex < 0 || dlIndex >= root.DownloaderConfig.Count)
            return null;
        var wsConfig = root.DownloaderConfig[dlIndex].WebsocketConfig;
        if (wsIndex < 0 || wsIndex >= wsConfig.Count)
            return null;
        return new DownloaderWebSocketDto
        {
            Id = wsId,
            DownloaderId = IndexToId(dlIndex),
            Host = wsConfig[wsIndex].Host,
            Port = wsConfig[wsIndex].Port
        };
    }

    public async Task<List<DownloaderWebSocketDto>> GetWebSocketsByDownloaderIdAsync(Guid downloaderId)
    {
        var dlIndex = IdToIndex(downloaderId);
        var root = await ReadRootAsync();
        if (dlIndex < 0 || dlIndex >= root.DownloaderConfig.Count)
            return new List<DownloaderWebSocketDto>();
        return root.DownloaderConfig[dlIndex].WebsocketConfig
            .Select((ws, wsIdx) => new DownloaderWebSocketDto
            {
                Id = WsIndexToId(dlIndex, wsIdx),
                DownloaderId = downloaderId,
                Host = ws.Host,
                Port = ws.Port
            }).ToList();
    }

    public async Task<DownloaderWebSocketDto> AddWebSocketAsync(Guid downloaderId, string host, int port)
    {
        var dlIndex = IdToIndex(downloaderId);
        var root = await ReadRootAsync();
        if (dlIndex < 0 || dlIndex >= root.DownloaderConfig.Count)
            throw new InvalidOperationException($"Downloader {downloaderId} not found.");
        root.DownloaderConfig[dlIndex].WebsocketConfig.Add(new WebSocketItemConfig { Host = host, Port = port });
        await WriteRootAsync(root);
        var wsIndex = root.DownloaderConfig[dlIndex].WebsocketConfig.Count - 1;
        return new DownloaderWebSocketDto
        {
            Id = WsIndexToId(dlIndex, wsIndex),
            DownloaderId = downloaderId,
            Host = host,
            Port = port
        };
    }

    public async Task<DownloaderWebSocketDto> UpdateWebSocketAsync(Guid wsId, string host, int port)
    {
        var (dlIndex, wsIndex) = ParseWsId(wsId);
        var root = await ReadRootAsync();
        if (dlIndex < 0 || dlIndex >= root.DownloaderConfig.Count ||
            wsIndex < 0 || wsIndex >= root.DownloaderConfig[dlIndex].WebsocketConfig.Count)
            throw new InvalidOperationException($"WebSocket {wsId} not found.");
        root.DownloaderConfig[dlIndex].WebsocketConfig[wsIndex] = new WebSocketItemConfig { Host = host, Port = port };
        await WriteRootAsync(root);
        return new DownloaderWebSocketDto
        {
            Id = wsId,
            DownloaderId = IndexToId(dlIndex),
            Host = host,
            Port = port
        };
    }

    public async Task DeleteWebSocketAsync(Guid wsId)
    {
        var (dlIndex, wsIndex) = ParseWsId(wsId);
        var root = await ReadRootAsync();
        if (dlIndex < 0 || dlIndex >= root.DownloaderConfig.Count ||
            wsIndex < 0 || wsIndex >= root.DownloaderConfig[dlIndex].WebsocketConfig.Count)
            throw new InvalidOperationException($"WebSocket {wsId} not found.");
        root.DownloaderConfig[dlIndex].WebsocketConfig.RemoveAt(wsIndex);
        await WriteRootAsync(root);
    }
}
