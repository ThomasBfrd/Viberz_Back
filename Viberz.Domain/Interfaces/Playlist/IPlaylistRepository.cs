using Viberz.Domain.Entities;
using Viberz.Domain.Enums;
using Viberz.Domain.Interfaces;

public interface IPlaylistRepository : IBaseRepository<Playlist, int>
{
    Task<List<Playlist>> GetPlaylists(int page = 1, int pageSize = 5);
    Task<List<Playlist>> GetPlaylistsByFamily(string userId, FamilyGenres family, string search, bool likes, int page = 1, int pageSize = 5);
    Task<Playlist> UpdatePlaylist(Playlist playlist);
    Task<bool> LikePlaylist(int playlistId, string userId);
    Task<bool> DislikePlaylist(int playlistId, string userId);
}
