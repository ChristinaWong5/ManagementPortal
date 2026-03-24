using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ManagementPortal.Downloaders;

public class DownloaderRootConfig
{
    [JsonPropertyName("max_worker")]
    public int MaxWorker { get; set; }

    [JsonPropertyName("downloader_config")]
    public List<DownloaderItemConfig> DownloaderConfig { get; set; } = new();
}

public class DownloaderItemConfig
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("downstream_health_file")]
    public string DownstreamHealthFile { get; set; } = string.Empty;

    [JsonPropertyName("websocket_config")]
    public List<WebSocketItemConfig> WebsocketConfig { get; set; } = new();
}

public class WebSocketItemConfig
{
    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;

    [JsonPropertyName("port")]
    public int Port { get; set; }
}
