using MediatR;
using System.Linq;
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

public record GuessQuery(UserJwtConnexion Token, List<string>? DefinedGenres, Profile Profile, Activies GameType) : IRequest<RandomSongsDTO>;

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
        UserDTO? existingUser = new();
        
        if (request.Profile.Equals(Profile.User))
        {
            existingUser = await _userService.GetUserById(request.Token.UserId) ??
            throw new Exception("User not found in database");
        }

        List<GenresWithSpotifyId> genres;

        if (request.Profile.Equals(Profile.Guest))
        {
            genres = await _genresService.GetAllGuestGenresWithSpotifyId() ??
            throw new Exception("Guest genres not found in database");
        } else
        {
            genres = await _genresService.GetAllGenresWithSpotifyId() ??
                throw new Exception("Genres not found in database");
        }
        List<GenresWithSpotifyId> otherGenres = genres;

        if (request.DefinedGenres is not null && request.DefinedGenres.Count > 0)
        {
            genres = genres.Where(g => request.DefinedGenres.Contains(g.Name)).ToList();
        }

        IEnumerable<Task<RandomSong>> randomSongsTasks = Enumerable.Range(0, 5).Select(async _ =>
        {
            Random random = new();
            List<GenresWithSpotifyId> randomIndex = TakeRandom.TakeRandomToList(genres, 1, random);
            string randomPlaylistId = genres.Where(g => g.Name.Equals(randomIndex[0].Name)).First().SpotifyId;
            string randomGenre = genres.Where(g => g.Name.Equals(randomIndex[0].Name)).First().Name;
            List<GenresWithSpotifyId> randomOtherGenres = otherGenres
                .Where(g => !g.Name.Equals(randomGenre))
                .ToList();


            if (request.DefinedGenres is not null && request.DefinedGenres.Count > 0)
            {
                return await _guessService.GetSongFromPlaylist(request.Token.SpotifyJwt, request.Token.UserId, request.Profile, randomPlaylistId, randomGenre, randomOtherGenres, request.GameType);
            } else
            {
                List<GenresWithSpotifyId> otherGenres = TakeRandom.TakeRandomToList(randomOtherGenres, 3, random);

                return await _guessService.GetSongFromPlaylist(request.Token.SpotifyJwt, request.Token.UserId, request.Profile, randomPlaylistId, randomGenre, otherGenres, request.GameType);
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
