using Viberz.Application.Models;

namespace Viberz.Domain.Entities;

public class Artist
{
    public string[] Genres { get; set; } = [];
    public string Id { get; set; } = string.Empty;
    public SpotifyImage[] Images { get; set; } = [];
    public string Name { get; set; } = string.Empty;
}
