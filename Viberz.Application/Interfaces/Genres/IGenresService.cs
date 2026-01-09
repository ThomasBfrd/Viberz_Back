using Viberz.Application.DTO.Genres;

namespace Viberz.Application.Interfaces.Genres;

public interface IGenresService
{
    public Task<List<string>> GetAllGenres();
    public Task<List<GenresWithSpotifyId>> GetAllGenresWithSpotifyId();
    public Task<List<GenresWithSpotifyId>> GetAllGuestGenresWithSpotifyId();
}
