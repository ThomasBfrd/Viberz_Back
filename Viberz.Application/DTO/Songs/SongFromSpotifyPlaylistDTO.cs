using System.Text.Json.Serialization;

namespace Viberz.Application.DTO.Songs;

public class SongFromSpotifyPlaylistDTO
{
    public required TracksDto Tracks { get; set; }
}

public class TracksDto
{
    public required List<ItemDto> Items { get; set; }
}

public class ItemDto
{
    public required TrackDto Track { get; set; }
}

public class TrackDto
{
    public required AlbumDto Album { get; set; }
    public required List<ArtistDto> Artists { get; set; }
    public required string Id { get; set; }
    public required string Name { get; set; }
    [JsonPropertyName("duration_ms")]
    public int DurationMs { get; set; }
}

public class AlbumDto
{
    public required List<ImageDto> Images { get; set; }
}

public class ImageDto
{
    public required string Url { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
}

public class ArtistDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}
