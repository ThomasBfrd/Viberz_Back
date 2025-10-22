using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Models;

namespace Viberz.Application.Interfaces.Guess;

public interface IGuessService
{
    public Task<RandomSong> GetSongFromPlaylist(UserJwtConnexion token, string playlistId, string randomGenre, List<string> randomGenres);
}
