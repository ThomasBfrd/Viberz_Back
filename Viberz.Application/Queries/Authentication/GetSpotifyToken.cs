using MediatR;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Spotify;
using Viberz.Application.DTO.User;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Services.Authentification;

namespace Viberz.Application.Queries.Authentication;

public record GetSpotifyTokenQuery(SpotifyAuthCodeRequestDTO SpotifyAuthCodeRequest) : IRequest<AuthConnexionDTO>;
public class GetSpotifyToken : IRequestHandler<GetSpotifyTokenQuery, AuthConnexionDTO>
{
    private readonly ISpotifyService _spotifyAuthService;
    private readonly JwtService _jwtService;
    public GetSpotifyToken(ISpotifyService spotifyAuthService, JwtService jwtService)
    {
        _spotifyAuthService = spotifyAuthService;
        _jwtService = jwtService;
    }
    public async Task<AuthConnexionDTO> Handle(GetSpotifyTokenQuery request, CancellationToken cancellationToken)
    {
        SpotifyTokenDTO? tokens = await _spotifyAuthService.ExchangeSpotifyToken(request.SpotifyAuthCodeRequest);

        if (tokens == null)
        {
            throw new Exception("Failed to exchange Spotify token.");
        }

        UserSpotifyInformationsDTO user = await _spotifyAuthService.GetUserSpotifyInformations(tokens.AccessToken);

        if (user == null)
        {
            throw new Exception("Failed to retrieve Spotify user profile.");
        }

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
