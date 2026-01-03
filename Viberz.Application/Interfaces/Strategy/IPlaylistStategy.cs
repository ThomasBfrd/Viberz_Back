using Viberz.Application.DTO.Playlist;
using Viberz.Domain.Enums;

public interface IPlaylistStategy
{
    Task<List<PlaylistDTO>> BuildResult(string userId, bool? owner, int? playlistId, FamilyGenres? family, string? search, bool likes, int? page, int? pageSize);
}
