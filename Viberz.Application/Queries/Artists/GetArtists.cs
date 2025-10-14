using MediatR;
using Viberz.Application.DTO.Artists;
using Viberz.Application.Interfaces.Artists;
using Viberz.Application.Services.Artists;

namespace Viberz.Application.Queries.Artists;

public record GetArtistsQuery(string AccessToken, string SearchQuery) : IRequest<List<ArtistSearchDTO>>;
public class GetArtists : IRequestHandler<GetArtistsQuery, List<ArtistSearchDTO>>
{
    private readonly IArtistsService _artistsService;
    public GetArtists(IArtistsService artistsService)
    {
        _artistsService = artistsService;
    }
    public async Task<List<ArtistSearchDTO>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
    {
        return await _artistsService.GetArtistsFromSearch(request.AccessToken, request.SearchQuery);
    }
}
