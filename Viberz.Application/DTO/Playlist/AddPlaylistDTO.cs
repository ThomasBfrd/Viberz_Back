namespace Viberz.Application.DTO.Playlist;

public class AddPlaylistDTO
{
    public string Name { get; set; } = string.Empty;
    public List<string> GenreList { get; set; } = [];
    public string SpotifyPlaylistId { get; set; } = string.Empty;
}
