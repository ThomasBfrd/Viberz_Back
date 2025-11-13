using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Guess;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Models;
using Viberz.Application.Services.Redis;
using Viberz.Application.Utilities;
using Viberz.Domain.Enums;

namespace Viberz.Application.Services.GuessGenre;

public class GuessService : IGuessService
{
    private readonly ISpotifyService _spotifyService;
    private readonly RedisService _redisService;
    public GuessService(ISpotifyService spotifyService, RedisService redisService)
    {
        _spotifyService = spotifyService;
        _redisService = redisService;
    }
    public async Task<RandomSong> GetSongFromPlaylist(string token, string userId, string playlistId, string randomGenre, List<string>? otherRandomGenresName, List<GenresWithSpotifyId>? genres, Activies gameType)
    {
        Random random = new();
        SongFromSpotifyPlaylistDTO? songList = await _spotifyService.GetSongsPropsFromPlaylist(token, playlistId);

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

        if (gameType == Activies.GuessGenre)
        {
            return new()
            {
                Song = randomSongFromPlaylist[0],
                Genre = randomGenre,
                EarnedXp = XpGames.GuessGenre,
                OtherGenres = otherRandomGenresName
            };
        }

        if (genres is not null && genres.Count > 0 && gameType == Activies.GuessSong)
        {
            List<ItemDto> alreadySelectedSongs = new();
            List<RandomPropsSongDTO> randomOtherSongs = new();

            for (int i = 0; i < 3; i++)
            {
                List<GenresWithSpotifyId> randomGenres = TakeRandom.TakeRandomToList(genres, 1, random);

                SongFromSpotifyPlaylistDTO? otherRandomSongs = await _spotifyService.GetSongsPropsFromPlaylist(token, randomGenres[0].SpotifyId);

                if (otherRandomSongs?.Tracks.Items is null || otherRandomSongs.Tracks.Items.Count == 0)
                    throw new Exception("No songs found in the playlist");

                HashSet<string> alreadySelectedArtistIds = alreadySelectedSongs
                    .SelectMany(x => x.Track.Artists.Select(a => a.Id))
                    .Concat(randomSongFromPlaylist[0].Track.Artists.Select(a => a.Id))
                    .ToHashSet();

                List<ItemDto> availableSongs = otherRandomSongs.Tracks.Items
                    .Where(song =>
                        !alreadySelectedSongs.Any(x => x.Track.Id == song.Track.Id) &&
                        song.Track.Id != randomSongFromPlaylist[0].Track.Id &&
                        !song.Track.Artists.Any(a => alreadySelectedArtistIds.Contains(a.Id))
                    )
                    .ToList();

                if (availableSongs.Count == 0)
                    throw new Exception("No more unique songs available to select.");

                ItemDto randomOtherSong = TakeRandom.TakeRandomToList(availableSongs, 1, random)[0];
                alreadySelectedSongs.Add(randomOtherSong);

                randomOtherSongs.Add(new RandomPropsSongDTO
                {
                    Title = randomOtherSong.Track.Name,
                    Artists = randomOtherSong.Track.Artists.Select(a => a.Name).ToList()
                });
            }

            return new()
            {
                Song = randomSongFromPlaylist[0],
                Genre = randomGenre,
                EarnedXp = XpGames.GuessSong,
                OtherSongs = randomOtherSongs
            };
        }

        return null;
    }
}
