using Viberz.Application.DTO.Artists;

namespace Viberz.Application.Interfaces.Artists;

public interface IArtistsService
{
    public Task<List<ArtistSearchDTO>> GetArtistsFromSearch(string jwtToken, string search);
}
