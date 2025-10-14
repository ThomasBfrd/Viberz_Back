using Viberz.Domain.Entities;

namespace Viberz.Application.DTO.Artists;

public class ArtistSearchDTO
{
    public List<string> Genres { get; set; } = [];
    public string Id { get; set; } = string.Empty;
    public SpotifyImage[] Images { get; set; } = [];
    public string Name { get; set; } = string.Empty;
}