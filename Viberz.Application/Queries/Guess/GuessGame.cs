using MediatR;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Interfaces.Genres;
using Viberz.Application.Interfaces.Guess;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Models;
using Viberz.Application.Utilities;
using Viberz.Domain.Enums;

namespace Viberz.Application.Queries.Guess;

public record GuessQuery(UserJwtConnexion Token, List<string>? DefinedGenres, Activies GameType) : IRequest<RandomSongsDTO>;

public class GuessGame : IRequestHandler<GuessQuery, RandomSongsDTO>
{
    private readonly IGuessService _guessService;
    private readonly IUserService _userService;
    private readonly IGenresService _genresService;

    public GuessGame(IGuessService guessService, IUserService userService, IGenresService genresService)
    {
        _guessService = guessService;
        _userService = userService;
        _genresService = genresService;
    }

    public async Task<RandomSongsDTO> Handle(GuessQuery request, CancellationToken cancellationToken)
    {
        UserDTO? existingUser = await _userService.GetUserById(request.Token.UserId);

        if (existingUser is null) throw new Exception("User not found in database");

        List<GenresWithSpotifyId> genres = await _genresService.GetAllGenresWithSpotifyId();

        if (genres is null) throw new Exception("Can't get genres");

        List<GenresWithSpotifyId> otherGenres = genres;

        if (request.DefinedGenres is not null && request.DefinedGenres.Count > 0)
        {
            genres = genres.Where(g => request.DefinedGenres.Contains(g.Name)).ToList();
        }

        IEnumerable<Task<RandomSong>> randomSongsTasks = Enumerable.Range(0, 5).Select(async _ =>
        {
            Random random = new();
            int randomIndex = random.Next(genres.Count);
            string randomPlaylistId = genres[randomIndex].SpotifyId;
            string randomGenre = genres[randomIndex].Name;
            List<GenresWithSpotifyId> randomOtherGenres = otherGenres
                .Where(g => !g.Name.Equals(randomGenre))
                .ToList();


            if (request.DefinedGenres is not null && request.DefinedGenres.Count > 0)
            {
                return await _guessService.GetSongFromPlaylist(request.Token.SpotifyJwt, existingUser.User.Id, randomPlaylistId, randomGenre, randomOtherGenres, request.GameType);
            } else
            {
                List<GenresWithSpotifyId> otherGenres = TakeRandom.TakeRandomToList(randomOtherGenres, 3, random);

                return await _guessService.GetSongFromPlaylist(request.Token.SpotifyJwt, existingUser.User.Id, randomPlaylistId, randomGenre, otherGenres, request.GameType);
            }
        });

        List<RandomSong> randomSongs = (await Task.WhenAll(randomSongsTasks)).ToList();

        return new() 
        { 
            RandomSong = randomSongs,
            AccessToken = request.Token.SpotifyJwt
        };
    }
}
