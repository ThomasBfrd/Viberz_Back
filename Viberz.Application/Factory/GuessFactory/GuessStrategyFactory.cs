using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.Strategy;
using Viberz.Application.Strategy;
using Viberz.Domain.Enums;

namespace Viberz.Application.Factory.GuessFactory;

public class GuessStrategyFactory
{
    private readonly ISpotifyService _spotifyService;

    public GuessStrategyFactory(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }
    public IGuessStrategy GetStrategy(Activies gameType)
    {
        return gameType switch
        {
            Activies.GuessGenre => new GuessGenreStrategy(),
            Activies.GuessSong => new GuessSongStrategy(_spotifyService),
            _ => throw new NotImplementedException($"Strategy for {gameType} is not implemented.")
        };
    }
}
