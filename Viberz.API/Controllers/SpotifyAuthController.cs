using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.User;
using Viberz.Application.Queries;
using Viberz.Viberz.Infrastructure.Services;

namespace Viberz.Viberz.API.Controllers
{
    public class SpotifyAuthController : ControllerBase
    {
        private readonly SpotifyAuthService _spotifyAuthService;
        private readonly JwtService _jwtService;
        private readonly GetSpotifyUserInformations _getSpotifyUserInformations;

        public SpotifyAuthController(SpotifyAuthService spotifyAuthService, JwtService jwtService, HttpClient httpClient, GetSpotifyUserInformations getSpotifyUserInformations)
        {
            _spotifyAuthService = spotifyAuthService;
            _jwtService = jwtService;
            _getSpotifyUserInformations = getSpotifyUserInformations;
        }

        [HttpPost("getSpotifyToken")]
        public async Task<IActionResult> GetSpotifyToken([FromBody] SpotifyAuthCodeRequestDTO spotifyAuthCodeRequest)
        {
            var tokens = await _spotifyAuthService.ExchangeSpotifyTokenResponse(spotifyAuthCodeRequest.Code, spotifyAuthCodeRequest.RedirectUri);

            if (tokens == null)
            {
                return BadRequest("Failed to exchange Spotify token.");
            }

            UserSpotifyInformationsDTO user = await _getSpotifyUserInformations.getUserSpotifyInformations(tokens.AccessToken);

            if (user == null)
            {
                return BadRequest("Failed to retrieve Spotify user profile.");
            }

            string jwt = _jwtService.GenerateToken(user.Id, tokens.AccessToken);

            AuthConnexionDDTO result = new() {
                JwtToken = jwt,
                UserId = user.Id,
                RefreshToken = tokens.RefreshToken,
                ExpiresIn = tokens.ExpiresIn
            };

            return Ok(result);
        }

        [HttpPost("refreshSpotifyToken")]
        public async Task<IActionResult> RefreshSpotifyToken([FromBody] RefreshSpotifyTokenDTO refreshSpotifyTokenDTO)
        {
            SpotifyTokenResponse tokens = await _spotifyAuthService.RefreshSpotifyTokenResponse(refreshSpotifyTokenDTO) ?? 
                throw new Exception("Failed to refresh Spotify Token.");

            UserSpotifyInformationsDTO user = await _getSpotifyUserInformations.getUserSpotifyInformations(tokens.AccessToken);
            var jwt = _jwtService.GenerateToken(user.Id, tokens.AccessToken);

            AuthConnexionDDTO result = new() {
                JwtToken = jwt,
                UserId = user.Id,
                RefreshToken = tokens.RefreshToken,
                ExpiresIn = tokens.ExpiresIn
            };
            return Ok(result);
        }
    }
}

public class AuthConnexionDDTO
{
    public required string JwtToken { get; set; }
    public required string UserId { get; set; }
    public required string RefreshToken { get; set; }
    public required int ExpiresIn { get; set; }
}