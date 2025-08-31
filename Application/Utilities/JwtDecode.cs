

using System.IdentityModel.Tokens.Jwt;
using Viberz.Application.DTO.Auth;

namespace Viberz.Application.Utilities;

public class JwtDecode(JwtSecurityTokenHandler jwtSecurityTokenHandler)
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = jwtSecurityTokenHandler;

    public UserJwtConnexion GetUserAuthInformations(string authorizationHeader)
    {
        string jwt = RemoveBearerPrefix(authorizationHeader) ?? 
            throw new Exception("Authorization header is missing or does not contain a Bearer token.");

        var token = _jwtSecurityTokenHandler.ReadJwtToken(jwt) ?? throw new Exception("token manquant");

        return new()
        {
            SpotifyJwt = token.Claims.Single(s => s.Type == "spotify_access_token")?.Value!,
            UserId = token.Claims.Single(s => s.Type == "sub")?.Value!
        };
    }
    public static string? RemoveBearerPrefix(string authorizationHeader)
    {
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authorizationHeader.Replace("Bearer", "").Trim();
        }
        return null;
    }
}



