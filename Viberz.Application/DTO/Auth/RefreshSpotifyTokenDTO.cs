namespace Viberz.Application.DTO.Auth;

public class RefreshSpotifyTokenDTO
{
    public string RefreshToken { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
}
