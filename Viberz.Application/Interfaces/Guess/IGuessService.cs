using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Songs;

namespace Viberz.Application.Interfaces.Guess;

public interface IGuessService
{
    public Task<SongDTO> GetSongFromPlaylist(UserJwtConnexion token, string playlistId, string randomGenre, List<string> randomGenres);
}
