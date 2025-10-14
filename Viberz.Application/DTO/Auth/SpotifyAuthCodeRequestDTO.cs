namespace Viberz.Application.DTO.Auth
{
    public class SpotifyAuthCodeRequestDTO
    {
        public string Code { get; set; } = null!;
        public string RedirectUri { get; set; } = null!;
    }
}
