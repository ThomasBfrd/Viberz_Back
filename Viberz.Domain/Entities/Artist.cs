using System.Text.Json.Serialization;

namespace Viberz.Domain.Entities;

public class Artist
{
    public string[] Genres { get; set; } = [];
    public string Id { get; set; } = string.Empty;
    public SpotifyImage[] Images { get; set; } = [];
    public string Name { get; set; } = string.Empty;
}

public class SpotifyImage
{
    public int Id { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    [JsonPropertyName("height")]
    public int? Height { get; set; }
    [JsonPropertyName("width")]
    public int? Width { get; set; }
}
