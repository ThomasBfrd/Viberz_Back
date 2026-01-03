using MediatR;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.Interfaces.Playlist;
using Viberz.Domain.Enums;

namespace Viberz.Application.Queries.Playlists;

public record GetPlaylistsQuery(string userId, bool? owner, int? PlaylistId, FamilyGenres? Family, string? Search, bool Likes, int? Page, int? PageSize) : IRequest<List<PlaylistDTO>>;
public class GetPlaylists(IPlaylistService playlistInterface) : IRequestHandler<GetPlaylistsQuery, List<PlaylistDTO>>
{
    private readonly IPlaylistService _playlistInterface = playlistInterface;

    public async Task<List<PlaylistDTO>> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
    {
        List<PlaylistDTO> playlists = await _playlistInterface.GetPlaylist(request.userId, request.owner, request.PlaylistId, request.Family, request.Search, request.Likes, request.Page, request.PageSize);

        if (playlists == null || playlists.Count == 0)
        {
            return new List<PlaylistDTO>();
        }

        return playlists;
    }
}
