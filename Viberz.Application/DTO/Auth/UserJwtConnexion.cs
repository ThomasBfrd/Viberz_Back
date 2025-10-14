namespace Viberz.Application.DTO.Auth;

public class UserJwtConnexion
{
    public required string SpotifyJwt { get; set; } = string.Empty;
    public required string UserId { get; set; } = string.Empty;
}
