using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.Strategy;
using Viberz.Application.Models;
using Viberz.Application.Utilities;
using Viberz.Domain.Enums;

namespace Viberz.Application.Strategy.Guess;

public class GuessSongStrategy : IGuessStrategy
{
    private readonly ISpotifyService _spotifyService;

    public GuessSongStrategy(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }
    public async Task<RandomSong> BuildResult
    (
        string token,
        ItemDto randomSongFromPlaylist,
        string genre,
        List<GenresWithSpotifyId> otherGenres
    )
    {
        if (otherGenres.Count == 0)
            throw new ArgumentException("Genres list is required for GuessSong strategy");

        Random random = new();
        List<ItemDto> alreadySelectedSongs = new();
        List<RandomPropsSongDTO> randomOtherSongs = new();

        for (int i = 0; i < 3; i++)
        {
            List<GenresWithSpotifyId> randomGenres = TakeRandom.TakeRandomToList(otherGenres, 1, random);

            SongFromSpotifyPlaylistDTO? otherRandomSongs = await _spotifyService.GetSongFromSpotifyPlaylist(token, randomGenres[0].SpotifyId);

            if (otherRandomSongs?.Tracks.Items is null || otherRandomSongs.Tracks.Items.Count == 0)
                throw new Exception("No songs found in the playlist");

            HashSet<string> alreadySelectedArtistIds = alreadySelectedSongs
                .SelectMany(x => x.Track.Artists.Select(a => a.Id))
                .Concat(randomSongFromPlaylist.Track.Artists.Select(a => a.Id))
                .ToHashSet();

            List<ItemDto> availableSongs = otherRandomSongs.Tracks.Items
                .Where(song =>
                    !alreadySelectedSongs.Any(x => x.Track.Id == song.Track.Id) &&
                    song.Track.Id != randomSongFromPlaylist.Track.Id &&
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

        return new RandomSong
        {
            Song = randomSongFromPlaylist,
            Genre = genre,
            EarnedXp = XpGames.GuessSong,
            OtherSongs = randomOtherSongs
        };
    }
}
