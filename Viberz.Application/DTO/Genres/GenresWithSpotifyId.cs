namespace Viberz.Application.DTO.Genres;

public class GenresWithSpotifyId
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SpotifyId { get; set; } = string.Empty;
}
