using MediatR;
using Viberz.Application.DTO.Auth;
using Viberz.Application.Services.Authentification;

namespace Viberz.Application.Queries.Authentication;

public record GetGenericSpotifyTokenQuery(GenericAuthRequestDTO GenericAuthRequest) : IRequest<AuthConnexionDTO>;
public class GetGenericToken(JwtService jwtService) : IRequestHandler<GetGenericSpotifyTokenQuery, AuthConnexionDTO>
{
    private readonly JwtService _jwtService = jwtService;
    public async Task<AuthConnexionDTO> Handle(GetGenericSpotifyTokenQuery request, CancellationToken cancellationToken)
    {
        string jwt = _jwtService.GenerateToken();

        return new()
        {
            JwtToken = jwt,
            UserId = "",
            RefreshToken = "",
            ExpiresIn = 3600
        };
    }
}
