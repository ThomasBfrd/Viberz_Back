using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Songs;
using Viberz.Application.DTO.Spotify;
using Viberz.Application.DTO.User;
using Viberz.Application.Interfaces.Spotify;

namespace Viberz.Application.Services.Spotify;

public class SpotifyService : ISpotifyService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SpotifyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<SpotifyTokenDTO?> ExchangeSpotifyToken(SpotifyAuthCodeRequestDTO spotifyAuthCodeRequest)
    {
        string clientId = _configuration["Spotify:ClientId"] ?? string.Empty;
        string clientSecret = _configuration["Spotify:ClientSecret"] ?? string.Empty;

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        string basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
         
        Dictionary<string, string> content = new()
        {
            { "grant_type", "authorization_code" },
            { "code", spotifyAuthCodeRequest.Code },
            { "redirect_uri", spotifyAuthCodeRequest.RedirectUri }
        };

        request.Content = new FormUrlEncodedContent(content);

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string errorJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Spotify token error: " + errorJson);
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();

        SpotifyTokenDTO? tokens = JsonSerializer.Deserialize<SpotifyTokenDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return tokens;
    }

    public async Task<SpotifyTokenDTO?> RefreshSpotifyToken(RefreshSpotifyTokenDTO refreshSpotifyTokenDTO)
    {
        string clientId = _configuration["Spotify:ClientId"] ?? string.Empty;
        string clientSecret = _configuration["Spotify:ClientSecret"] ?? string.Empty;

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        string basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

        Dictionary<string, string> content = new()
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshSpotifyTokenDTO.RefreshToken },
            { "client_id", refreshSpotifyTokenDTO.ClientId }
        };

        request.Content = new FormUrlEncodedContent(content);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string errorJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Spotify refresh token error: " + errorJson);
            return null;
        }
        string json = await response.Content.ReadAsStringAsync();

        SpotifyTokenDTO tokens = JsonSerializer.Deserialize<SpotifyTokenDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
            throw new Exception("Failed to deserialize the token");
        return tokens;
    }

    public async Task<UserSpotifyInformationsDTO> GetUserSpotifyInformations(string accessToken)
    {
        HttpRequestMessage request = new(HttpMethod.Get, "https://api.spotify.com/v1/me");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception($"Cannot get user's informations : {errorJson}");
        }

        string json = await response.Content.ReadAsStringAsync();

        UserSpotifyInformationsDTO? userProfile = JsonSerializer.Deserialize<UserSpotifyInformationsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (userProfile is null)
        {
            throw new Exception("Failed to deserialize the user profile");
        }

        if (userProfile.UserProduct != "premium")
        {
            throw new Exception("You must have a premium account to use Viberz.");
        }

        return userProfile;
    }

    public async Task<SongFromSpotifyPlaylistDTO> GetSongsPropsFromPlaylist(string spotifyJwt, string playlistId)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"https://api.spotify.com/v1/playlists/{Uri.UnescapeDataString(playlistId)}?fields=tracks%28items%28track%28album%28name%2Cimages%29%2Cname%2Cid%2C+duration_ms%2Cartists%28id%2Cname%29%29%29%29");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", spotifyJwt);

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string otherErrorJson = await response.Content.ReadAsStringAsync();
            throw new Exception($"Can't get this playlist : {otherErrorJson}");
        }

        string json = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(json) || json is null)
        {
            throw new Exception("Playlist is empty");
        }

        SongFromSpotifyPlaylistDTO? randomSongs = JsonSerializer.Deserialize<SongFromSpotifyPlaylistDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (randomSongs?.Tracks.Items is null || randomSongs.Tracks.Items.Count == 0)
        {
            throw new Exception("No songs found in the playlist");
        }

        return randomSongs;
    }
}
