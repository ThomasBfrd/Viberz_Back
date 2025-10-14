using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Spotify;
using Viberz.Application.DTO.User;

namespace Viberz.Application.Interfaces.Spotify;

public interface ISpotifyService
{
    public Task<SpotifyTokenDTO?> ExchangeSpotifyToken(SpotifyAuthCodeRequestDTO spotifyAuthCodeRequest);
    public Task<SpotifyTokenDTO?> RefreshSpotifyToken(RefreshSpotifyTokenDTO refreshSpotifyTokenDTO);
    public Task<UserSpotifyInformationsDTO> GetUserSpotifyInformations(string accessToken);
}
