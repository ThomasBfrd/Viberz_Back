using Viberz.Domain.Entities;

namespace Viberz.Domain.Interfaces.LikedPlaylistRepository;

public interface ILikedPlaylistsRepository : IBaseRepository<LikedPlaylists, int>
{
    public Task<LikedPlaylists?> GetLikedPlaylistAsync(int playlistId, string userId);
    public Task<bool> IsPlaylistLikedAsync(int playlistId, string userId);
}
