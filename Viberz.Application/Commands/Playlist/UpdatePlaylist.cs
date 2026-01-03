using MediatR;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.Interfaces.Playlist;

namespace Viberz.Application.Commands.Playlist;

public class UpdatePlaylist : IRequest<PlaylistDTO>
{
    public readonly UpdatePlaylistDTO PlaylistData;
    public readonly string UserId;

    public UpdatePlaylist(UpdatePlaylistDTO updatePlaylist, string userId)
    {
        PlaylistData = updatePlaylist;
        UserId = userId;
    }

    public class Handler(IPlaylistService playlistService) : IRequestHandler<UpdatePlaylist, PlaylistDTO>
    {
        private readonly IPlaylistService _playlistService = playlistService;
        public async Task<PlaylistDTO> Handle(UpdatePlaylist request, CancellationToken cancellationToken)
        {
            return await _playlistService.UpdatePlaylist(request.PlaylistData, request.UserId);
        }
    }
}
