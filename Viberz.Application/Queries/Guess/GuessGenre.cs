using MediatR;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Genres;
using Viberz.Application.Interfaces.Guess;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Models;
using Viberz.Application.Utilities;

namespace Viberz.Application.Queries.Guess;

public record GuessGenreQuery(UserJwtConnexion token) : IRequest<RandomSongsDTO>;

public class GuessGenre : IRequestHandler<GuessGenreQuery, RandomSongsDTO>
{
    private readonly IGuessService _guessService;
    private readonly IUserService _userService;
    private readonly IGenresService _genresService;

    public GuessGenre(IGuessService guessService, IUserService userService, IGenresService genresService)
    {
        _guessService = guessService;
        _userService = userService;
        _genresService = genresService;
    }

    public async Task<RandomSongsDTO> Handle(GuessGenreQuery request, CancellationToken cancellationToken)
    {
        UserDTO? existingUser = await _userService.GetUserById(request.token.UserId);

        if (existingUser is null) throw new Exception("User not found in database");

        List<GenresWithSpotifyId> genres = await _genresService.GetAllGenresWithSpotifyId();

        if (genres is null) throw new Exception("Can't get genres");

        var randomSongsTasks = Enumerable.Range(0, 5).Select(async _ =>
        {
        Random random = new();
            int randomIndex = random.Next(genres.Count);
            string randomPlaylistId = genres[randomIndex].SpotifyId;
            string randomGenre = genres[randomIndex].Name;
            List<string> randomOtherGenres = genres
                .Select(a => a.Name)
                .Where(g => !g.Equals(randomGenre))
                .ToList();

            List<string> otherGenres = TakeRandom.TakeRandomToList(randomOtherGenres, 3, random);
            
            return await _guessService.GetSongFromPlaylist(request.token, randomPlaylistId, randomGenre, otherGenres);
        });

        List<RandomSong> randomSongs = (await Task.WhenAll(randomSongsTasks)).ToList();

        return new() 
        { 
            RandomSong = randomSongs,
            AccessToken = request.token.SpotifyJwt
        };
    }
}
