using Viberz.Application.DTO.Playlist;
using Viberz.Domain.Enums;

namespace Viberz.Application.Interfaces.Playlist;

public interface IPlaylistService
{
    Task<List<PlaylistDTO>> GetPlaylist(string userId, bool? owner, int? playlistId, FamilyGenres? family, string? search, bool likes, int? page, int? pageSize);
    Task<PlaylistDTO> CreatePlaylist(AddPlaylistDTO addPlaylist, string userId);
    Task<bool> DeletePlaylist(int playlistId, string userId);
    Task<PlaylistDTO> UpdatePlaylist(UpdatePlaylistDTO playlistToUpdate, string userId);
    Task<bool> LikePlaylist(int playlistId, string userId);
}