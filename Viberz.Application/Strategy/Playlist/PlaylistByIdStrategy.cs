using AutoMapper;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Services.Redis;
using Viberz.Domain.Entities;
using Viberz.Domain.Enums;
using Viberz.Domain.Interfaces.LikedPlaylistRepository;

public class PlaylistByIdStrategy(
    ISpotifyService spotifyService, 
    IUserService userService, 
    IPlaylistRepository playlistRepository,
    ILikedPlaylistsRepository likedPlaylistsRepository,
    RedisService redisService,
    IMapper mapper) : IPlaylistStategy
{
    private readonly ISpotifyService _spotifyService = spotifyService;
    private readonly IUserService _userService = userService;
    private readonly IPlaylistRepository _playlistRepository = playlistRepository;
    private readonly ILikedPlaylistsRepository _likedPlaylistsRepository = likedPlaylistsRepository;
    private readonly RedisService _redisService = redisService;
    private readonly IMapper _mapper = mapper;
    public async Task<List<PlaylistDTO>> BuildResult(string userId, bool? owner, int? playlistId, FamilyGenres? families, string? search, bool likes, int? page, int? pageSize)
    {
        if (!playlistId.HasValue || playlistId <= 0) 
            throw new Exception("The given playlistId is invalid");

        Playlist playlist = await _playlistRepository.GetByIdAsync(playlistId.Value) ??
            throw new ArgumentException("The given playlistId does not exist");

        bool isLikedByUser = false;

        if (userId.Length > 0)
        {
            UserDTO? user = await _userService.GetUserById(userId) ?? 
                throw new Exception("User not found");

            isLikedByUser = await _likedPlaylistsRepository.IsPlaylistLikedAsync(playlist.Id, user.User.Id);
        }

        PlaylistDTO existingPlaylists = _mapper.Map<PlaylistDTO>(playlist);
        SongFromSpotifyPlaylistDTO spotifyPlaylist;
        bool isCached = _redisService.CachedPlaylist(existingPlaylists.SpotifyPlaylistId);

        if (isCached)
        {
            spotifyPlaylist = _redisService.GetCachedPlaylist(existingPlaylists.SpotifyPlaylistId);
        } else
        {
            spotifyPlaylist = await _spotifyService.GetSongFromSpotifyPlaylist(null, existingPlaylists.SpotifyPlaylistId);
            _redisService.AddPlaylist(existingPlaylists.SpotifyPlaylistId, spotifyPlaylist);
        }

        if (spotifyPlaylist is null || spotifyPlaylist.Tracks.Items.Count == 0)
            throw new ArgumentException("The given playlistId from Spotify does not exist or has no songs");

        UserDTO? playlistOwner = await _userService.GetUserById(playlist.UserId) ??
                    throw new Exception("Owner of this playlist not found");

        if (playlistOwner.User.Username is null) throw new ArgumentException("The user doesn't have a proper username");

        existingPlaylists.UserId = playlistOwner.User.Id;
        existingPlaylists.UserName = playlistOwner.User.Username;
        existingPlaylists.Songs = spotifyPlaylist;
        existingPlaylists.LikedByUser = isLikedByUser;
        existingPlaylists.UserImageProfile = playlistOwner.User.Image;

        return new List<PlaylistDTO> { existingPlaylists };
    }
}
