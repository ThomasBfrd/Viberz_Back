using MediatR;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Genres;
using Viberz.Application.Interfaces.Guess;
using Viberz.Application.Interfaces.User;

namespace Viberz.Application.Queries.Guess;

public record GuessGenreQuery(UserJwtConnexion token) : IRequest<SongDTO>;

public class GuessGenre : IRequestHandler<GuessGenreQuery, SongDTO>
{
    private readonly IGuessService _guess;
    private readonly IUserService _userService;
    private readonly IGenresService _genresService;

    public GuessGenre(IGuessService guess, IUserService userService, IGenresService genresService)
    {
        _guess = guess;
        _userService = userService;
        _genresService = genresService;
    }

    public async Task<SongDTO> Handle(GuessGenreQuery request, CancellationToken cancellationToken)
    {
        UserDTO? existingUser = await _userService.GetUserById(request.token.UserId);

        if (existingUser is null) throw new Exception("User not found in database");

        List<GenresWithSpotifyId> genres = await _genresService.GetAllGenresWithSpotifyId();

        if (genres is null) throw new Exception("Can't get genres");

        Random random = new();

        int randomIndex = random.Next(genres.Count);
        string randomPlaylistId = genres[randomIndex].SpotifyId;
        string randomGenre = genres[randomIndex].Name;
        List<string> randomOtherGenres = genres
            .Select(a => a.Name)
            .Where(g => !g.Equals(randomGenre))
            .ToList();

        List<string> otherGenres = randomOtherGenres.OrderBy(x => random.Next()).Take(3).ToList();

        SongDTO songFromPlaylist = await _guess.GetSongFromPlaylist(request.token, randomPlaylistId, randomGenre, otherGenres);

        return songFromPlaylist;
    }
}
