using MediatR;
using Viberz.Application.Interfaces.Genres;

namespace Viberz.Application.Queries.Genres;

public record GetGenresQuery : IRequest<List<string>>;

public class GetGenres : IRequestHandler<GetGenresQuery, List<string>>
{
    private readonly IGenresService _genresService;

    public GetGenres(IGenresService genresService)
    {
        _genresService = genresService;
    }

    public async Task<List<string>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        return await _genresService.GetAllGenres();
    }
}
