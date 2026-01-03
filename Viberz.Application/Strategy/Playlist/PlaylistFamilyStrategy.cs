using AutoMapper;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.User;
using Viberz.Domain.Entities;
using Viberz.Domain.Enums;
using Viberz.Domain.Interfaces.LikedPlaylistRepository;

public class PlaylistFamilyStrategy(
    ISpotifyService spotifyService, 
    IUserService userService, 
    IPlaylistRepository playlistRepository,
    ILikedPlaylistsRepository likedPlaylistsRepository,
    IMapper mapper) : IPlaylistStategy
{
    private readonly ISpotifyService _spotifyService = spotifyService;
    private readonly IUserService _userService = userService;
    private readonly IPlaylistRepository _playlistRepository = playlistRepository;
    private readonly ILikedPlaylistsRepository _likedPlaylistsRepository = likedPlaylistsRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<PlaylistDTO>> BuildResult(string userId, bool? owner, int? playlistId, FamilyGenres? family, string? search, bool likes, int? page, int? pageSize)
    {
        if (!family.HasValue) throw new Exception("No family");

        List<Playlist> playlists = await _playlistRepository.GetPlaylistsByFamily(userId, family.Value, search ?? "", likes, page ?? 1, pageSize ?? 5);

        List<PlaylistDTO> existingPlaylists = _mapper.Map<List<PlaylistDTO>>(playlists);
        List<PlaylistDTO> result = [];

        foreach (PlaylistDTO playlist in existingPlaylists) {

            UserDTO? user = await _userService.GetUserById(userId) ??
                throw new Exception("User not found");

            UserDTO? playlistOwner = await _userService.GetUserById(playlist.UserId) ??
                    throw new Exception("Owner of this playlist not found");

            if (playlistOwner.User.Username is null) throw new ArgumentException("The user doesn't have a proper username");

            bool isLikedByUser = await _likedPlaylistsRepository.IsPlaylistLikedAsync(playlist.Id, user.User.Id);

            playlist.LikedByUser = isLikedByUser;
            playlist.UserName = playlistOwner.User.Username;
            result.Add(playlist);
        }

        return result;
    }
}
