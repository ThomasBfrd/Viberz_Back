using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Genres;
using Viberz.Application.Models;
using Viberz.Domain.Enums;

namespace Viberz.Application.Interfaces.Guess;

public interface IGuessService
{
    public Task<RandomSong> GetSongFromPlaylist(string token, string playlistId, string randomGenre, List<string>? otherRandomGenresName, List<GenresWithSpotifyId>? genres, Activies gameType);
}

