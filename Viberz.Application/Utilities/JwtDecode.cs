

using System.IdentityModel.Tokens.Jwt;
using Viberz.Application.DTO.Auth;

namespace Viberz.Application.Utilities;

public class JwtDecode()
{
    public UserJwtConnexion GetUserAuthInformations(string authorizationHeader)
    {
        string jwt = RemoveBearerPrefix(authorizationHeader) ??
            throw new Exception("Authorization header is missing or does not contain a Bearer token.");

        JwtSecurityTokenHandler handler = new();
        var token = handler.ReadJwtToken(jwt) ?? throw new Exception("The JWT is not functionnal or some informations are missing.");

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



