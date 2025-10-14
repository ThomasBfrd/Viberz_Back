using MediatR;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Spotify;
using Viberz.Application.DTO.User;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Services.Authentification;

namespace Viberz.Application.Queries.Authentication;

public record RefreshSpotifyTokenQuery(RefreshSpotifyTokenDTO RefreshSpotifyTokenDTO) : IRequest<AuthConnexionDTO>;

public class RefreshSpotifyToken : IRequestHandler<RefreshSpotifyTokenQuery, AuthConnexionDTO>
{
    private readonly ISpotifyService _spotifyService;
    private readonly JwtService _jwtService;
    public RefreshSpotifyToken(ISpotifyService spotifyService, JwtService jwtService)
    {
        _spotifyService = spotifyService;
        _jwtService = jwtService;
    }

    public async Task<AuthConnexionDTO> Handle(RefreshSpotifyTokenQuery request, CancellationToken cancellationToken)
    {
        SpotifyTokenDTO tokens = await _spotifyService.RefreshSpotifyToken(request.RefreshSpotifyTokenDTO) ??
            throw new Exception("Failed to refresh Spotify Token.");

        UserSpotifyInformationsDTO user = await _spotifyService.GetUserSpotifyInformations(tokens.AccessToken);
        string jwt = _jwtService.GenerateToken(user.Id, tokens.AccessToken);

        return new()
        {
            JwtToken = jwt,
            UserId = user.Id,
            RefreshToken = tokens.RefreshToken,
            ExpiresIn = tokens.ExpiresIn
        };
    }
}
