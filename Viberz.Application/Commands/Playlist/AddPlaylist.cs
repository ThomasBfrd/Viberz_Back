using MediatR;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.Interfaces.Playlist;

namespace Viberz.Application.Commands.Playlist;

public class AddPlaylist : IRequest<PlaylistDTO>
{
    public readonly AddPlaylistDTO PlaylistData;
    public readonly string UserId;

    public AddPlaylist(AddPlaylistDTO addPlaylist, string userId)
    {
        PlaylistData = addPlaylist;
        UserId = userId;
    }

    public class Handler(IPlaylistService playlistService) : IRequestHandler<AddPlaylist, PlaylistDTO>
    {
        private readonly IPlaylistService _playlistService = playlistService;
        public async Task<PlaylistDTO> Handle(AddPlaylist request, CancellationToken cancellationToken)
        {
            return await _playlistService.CreatePlaylist(request.PlaylistData, request.UserId);
        }
    }
}
