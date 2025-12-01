using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Strategy;
using Viberz.Application.Models;
using Viberz.Domain.Enums;

namespace Viberz.Application.Strategy;

public class GuessGenreStrategy : IGuessStrategy
{
    public async Task<RandomSong> BuildResult
    (
        string token,
        string userId,
        ItemDto randomSongFromPlaylist,
        string genre,
        List<GenresWithSpotifyId> otherGenres
    )
    {
        return new RandomSong
        {
            Song = randomSongFromPlaylist,
            Genre = genre,
            EarnedXp = XpGames.GuessGenre,
            OtherGenres = otherGenres?.Select(g => g.Name).ToList()
        };
    }
}
