using AutoMapper;
using Viberz.Application.DTO.Genres;
using Viberz.Application.Interfaces.Genres;
using Viberz.Domain.Entities;
using Viberz.Domain.Interfaces.Genres;

namespace Viberz.Application.Services.Genres;

public class GenresService : IGenresService
{
    private readonly IGenresRepository _genresRepository;
    private readonly IMapper _mapper;

    public GenresService(IGenresRepository genresRepository, IMapper mapper)
    {
        _genresRepository = genresRepository;
        _mapper = mapper;
    }

    public async Task<List<string>> GetAllGenres()
    {
        return await _genresRepository.GetAllGenres();
    }

    public async Task<List<GenresWithSpotifyId>> GetAllGenresWithSpotifyId()
    {
        List<Genre> genres = await _genresRepository.GetAllGenresWithSpotifyId();

        List<GenresWithSpotifyId> genresDto = _mapper.Map<List<GenresWithSpotifyId>>(genres);

        return genresDto;
    }

    public async Task<List<GenresWithSpotifyId>> GetAllGuestGenresWithSpotifyId()
    {
        List<Genre> genres = await _genresRepository.GetAllGuestGenresWithSpotifyId();
        List<GenresWithSpotifyId> genresDto = _mapper.Map<List<GenresWithSpotifyId>>(genres);
        return genresDto;
    }
}
