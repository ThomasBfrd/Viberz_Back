using Viberz.Application.DTO.Songs;

namespace Viberz.Application.DTO.Playlist;

public class PlaylistDTO
{
    public int Id { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string SpotifyPlaylistId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserImageProfile { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> GenreList { get; set; } = [];
    public SongFromSpotifyPlaylistDTO? Songs { get; set; }
    public string Image { get; set; } = string.Empty;
    public int Likes { get; set; } = 0;
    public string UserName { get; set; } = string.Empty;
    public bool LikedByUser { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
