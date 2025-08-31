using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using Viberz.Application.DTO;
using Viberz.Application.DTO.Artists;
using Viberz.Domain.Entities;

namespace Viberz.Application.Queries;

public class GetArtistsFromSearch
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;
    public GetArtistsFromSearch(HttpClient httpClient, GetSpotifyUserInformations getSpotifyUserInformations, ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    public async Task<List<ArtistSearchDTO>> GetArtists(string spotifyAccessToken, string searchQuery)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(searchQuery)}&type=artist&limit=3");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", spotifyAccessToken);
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error fetching artists: {errorJson}");
        }

        string json = await response.Content.ReadAsStringAsync();
        JsonDocument convert = JsonDocument.Parse(json);
        JsonElement artistsList = convert.RootElement.GetProperty("artists").GetProperty("items");

        var artistsResponse = JsonSerializer.Deserialize<List<ArtistSearchDTO>>(artistsList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new Exception("Fail to get artists list");


        foreach (var artist in artistsResponse)
        {
            List<string> artistGenres = [];

            if (artist.Genres is not null)
            {
                foreach (string genre in artist.Genres)
                {
                    var resultGenre = await _context.Genres
                        .FirstOrDefaultAsync(g => g.Name.ToLower() == genre.ToString().ToLower());

                    if (resultGenre is null) continue;

                    if (artistGenres.Count < 3)
                    {
                        artistGenres.Add(resultGenre.Name);
                    } else
                    {
                        continue;
                    }
                 }
            }

            artist.Genres = artistGenres;
        }

        return artistsResponse;
    }
}
