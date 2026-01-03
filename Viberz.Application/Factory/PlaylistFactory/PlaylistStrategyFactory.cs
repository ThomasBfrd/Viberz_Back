using AutoMapper;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Services.Redis;
using Viberz.Domain.Enums;
using Viberz.Domain.Interfaces.LikedPlaylistRepository;

namespace Viberz.Application.Factory.PlaylistFactory;

public class PlaylistStrategyFactory(
    ISpotifyService spotifyService, 
    IUserService userService, 
    IPlaylistRepository playlistRepository, 
    ILikedPlaylistsRepository likedPlaylistsRepository,
    RedisService redisService,
    IMapper mapper)
{
    private readonly ISpotifyService _spotifyService = spotifyService;
    private readonly IUserService _userService = userService;
    private readonly IPlaylistRepository _playlistRepository = playlistRepository;
    private readonly ILikedPlaylistsRepository _likedPlaylistsRepository = likedPlaylistsRepository;
    private readonly RedisService _redisService = redisService;
    private readonly IMapper _mapper = mapper;

    public IPlaylistStategy GetStategy(bool? owner, int? spotifyId, FamilyGenres? family, int? page, int? pageSize)
    {
        if (spotifyId is not null) return new PlaylistByIdStrategy(_spotifyService, _userService, _playlistRepository, _likedPlaylistsRepository, _redisService, _mapper);

        if (family is not null) return new PlaylistFamilyStrategy(_spotifyService, _userService, _playlistRepository, _likedPlaylistsRepository, _mapper);

        return new PlaylistGenericStrategy(_spotifyService, _userService, _playlistRepository, _likedPlaylistsRepository, _mapper);
    }
}
