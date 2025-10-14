using Viberz.Domain.Entities;

namespace Viberz.Domain.Interfaces.Genres;

public interface IGenresRepository
{
    public Task<List<string>> GetAllGenres();
    public Task<List<Genre>> GetAllGenresWithSpotifyId();
}
