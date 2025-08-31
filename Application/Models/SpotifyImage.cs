using System.Text.Json.Serialization;

namespace Viberz.Application.Models;

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
