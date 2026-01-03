namespace Viberz.Application.DTO.Playlist;

public class UpdatePlaylistDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SpotifyPlaylistId { get; set; } = string.Empty;
    public List<string> GenreList { get; set; } = [];
    public int Likes { get; set; } = 0;
}