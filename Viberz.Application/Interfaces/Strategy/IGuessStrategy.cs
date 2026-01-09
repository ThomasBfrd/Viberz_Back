using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Models;

namespace Viberz.Application.Interfaces.Strategy;

public interface IGuessStrategy
{
    Task<RandomSong> BuildResult
    (
        string token,
        ItemDto randomSongFromPlaylist,
        string genre,
        List<GenresWithSpotifyId> genres
    );
}
