using MediatR;
using Viberz.Application.Interfaces.Playlist;

namespace Viberz.Application.Commands.Playlist;

public class LikePlaylist : IRequest<bool>
{
    public int PlaylistId { get; }
    public string UserId { get; }

    public LikePlaylist(int playlistId, string userId)
    {
        PlaylistId = playlistId;
        UserId = userId;
    }

    public class Handler : IRequestHandler<LikePlaylist, bool>
    {
        private readonly IPlaylistService _playlistService;
        public Handler(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        public async Task<bool> Handle(LikePlaylist request, CancellationToken cancellationToken)
        {
            bool result = await _playlistService.LikePlaylist(request.PlaylistId, request.UserId);

            return result;
        }
    }
}
