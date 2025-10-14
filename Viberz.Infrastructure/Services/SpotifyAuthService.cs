using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Viberz.Domain.Models;

namespace Viberz.Infrastructure.Services;

public class SpotifyAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SpotifyAuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<SpotifyTokenResponse?> ExchangeSpotifyTokenResponse(string code, string redirectUri)
    {
        string clientId = _configuration["Spotify:ClientId"] ?? string.Empty;
        string clientSecret = _configuration["Spotify:ClientSecret"] ?? string.Empty;

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

        var content = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", redirectUri }
        };

        request.Content = new FormUrlEncodedContent(content);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Spotify token error: " + errorJson);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        var tokens = JsonSerializer.Deserialize<SpotifyTokenResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return tokens;
    }

    public async Task<SpotifyTokenResponse?> RefreshSpotifyTokenResponse(SpotifyTokenResponse refreshSpotifyTokenDTO)
    {
        string clientId = _configuration["Spotify:ClientId"] ?? string.Empty;
        string clientSecret = _configuration["Spotify:ClientSecret"] ?? string.Empty;

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

        var content = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshSpotifyTokenDTO.RefreshToken },
            { "client_id", clientId }
        };

        request.Content = new FormUrlEncodedContent(content);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Spotify refresh token error: " + errorJson);
            return null;
        }
        var json = await response.Content.ReadAsStringAsync();

        SpotifyTokenResponse tokens = JsonSerializer.Deserialize<SpotifyTokenResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
            throw new Exception("Failed to deserialize the token");
        return tokens;
    }
}