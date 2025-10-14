using System.Net.Http.Headers;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Viberz.Application.DTO.User;

namespace Viberz.Application.Queries;

public class GetSpotifyUserInformations
{
    private readonly HttpClient _httpClient;

    public GetSpotifyUserInformations(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserSpotifyInformationsDTO> getUserSpotifyInformations(string accessToken)
    {
        HttpRequestMessage request = new (HttpMethod.Get, "https://api.spotify.com/v1/me");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        var userProfile = JsonSerializer.Deserialize<UserSpotifyInformationsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (userProfile is null)
        {
            return null;
        }

        return userProfile;
    }
}
