using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Factory.GuessFactory;
using Viberz.Application.Interfaces.Guess;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.Strategy;
using Viberz.Application.Models;
using Viberz.Application.Services.Redis;
using Viberz.Application.Utilities;
using Viberz.Domain.Enums;

namespace Viberz.Application.Services.GuessGenre;

public class GuessService : IGuessService
{
    private readonly ISpotifyService _spotifyService;
    private readonly RedisService _redisService;
    private GuessStrategyFactory _strategyFactory;
    public GuessService(ISpotifyService spotifyService, RedisService redisService, GuessStrategyFactory guessStrategyFactory)
    {
        _spotifyService = spotifyService;
        _redisService = redisService;
        _strategyFactory = guessStrategyFactory;
    }
    public async Task<RandomSong> GetSongFromPlaylist(string token, string userId, string playlistId, string randomGenre, List<GenresWithSpotifyId> otherGenres, Activies gameType)
    {
        Random random = new();
        SongFromSpotifyPlaylistDTO? songList = await _spotifyService.GetSongFromSpotifyPlaylist(token, playlistId);

        if (songList?.Tracks.Items is null || songList.Tracks.Items.Count == 0)
        {
            throw new Exception("No songs found in the playlist");
        }

        List<ItemDto> randomSongFromPlaylist = [];

        do
        {
            randomSongFromPlaylist = TakeRandom.TakeRandomToList(songList.Tracks.Items, 1, random);
        }
        while (_redisService.SongAlreadyPlayed(userId, randomSongFromPlaylist[0].Track.Id));

        _redisService.AddSongForUser(userId, randomSongFromPlaylist[0].Track.Id);

        IGuessStrategy strategy = _strategyFactory?.GetStrategy(gameType)!;

        return await strategy.BuildResult(token, userId, randomSongFromPlaylist[0], randomGenre, otherGenres);
    }
}
