using System.Text.Json.Serialization;

namespace Viberz.Application.DTO.Auth;

public class AuthConnexionDTO
{

    public string JwtToken { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}
