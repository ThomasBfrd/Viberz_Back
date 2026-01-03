using MediatR;
using Viberz.Application.Interfaces.Playlist;

namespace Viberz.Application.Commands.Playlist;

public class DeletePlaylist : IRequest<bool>
{
    public int PlaylistId { get; }
    public string UserId { get; }
    public DeletePlaylist(int playlistId, string userId)
    {
        PlaylistId = playlistId;
        UserId = userId;
    }

    public class Handler (IPlaylistService playlistService) : IRequestHandler<DeletePlaylist, bool>
    {
        private readonly IPlaylistService _playlistService = playlistService;
        public async Task<bool> Handle(DeletePlaylist request, CancellationToken cancellationToken)
        {
            return await _playlistService.DeletePlaylist(request.PlaylistId, request.UserId);
        }
    }
}
