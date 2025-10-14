using System.Net.Http.Headers;
using System.Text.Json;
using Viberz.Application.DTO.Artists;
using Viberz.Application.Interfaces.Artists;
using Viberz.Application.Interfaces.Genres;

namespace Viberz.Application.Services.Artists;

public class ArtistsService : IArtistsService
{
    private readonly HttpClient _httpClient;
    private readonly IGenresService _genresService;

    public ArtistsService(HttpClient httpClient, IGenresService genresService)
    {
        _httpClient = httpClient;
        _genresService = genresService;
    }

    public async Task<List<ArtistSearchDTO>> GetArtistsFromSearch(string spotifyAccessToken, string searchQuery)
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

        List<ArtistSearchDTO> artistsResponse = JsonSerializer.Deserialize<List<ArtistSearchDTO>>(artistsList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new Exception("Fail to get artists list");

        foreach (var artist in artistsResponse)
        {
            List<string> artistGenres = [];

            if (artist.Genres is not null)
            {
                foreach (string genre in artist.Genres)
                {
                    List<string> genresList = await _genresService.GetAllGenres();

                    string? resultGenre = genresList.FirstOrDefault(g => g.ToLower() == genre.ToString().ToLower());

                    if (resultGenre is null) continue;

                    if (artistGenres.Count < 3)
                    {
                        artistGenres.Add(resultGenre);
                    }
                    else
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
