using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using System.Net.Http.Headers;
using System.Text.Json;
using Viberz.Application.DTO.Songs;
using Viberz.Domain.Enums;

namespace Viberz.Application.Queries;

public class SongFromPlaylist
{
    private readonly HttpClient _httpClient;

    public SongFromPlaylist(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SongDTO> GetSongFromPlaylist(string SpotifyJwt, string PlaylistId, string GenreName, List<string> otherGenres)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"https://api.spotify.com/v1/playlists/{Uri.UnescapeDataString(PlaylistId)}?fields=tracks%28items%28track%28album%28name%2Cimages%29%2Cname%2Cid%2C+duration_ms%2Cartists%28id%2Cname%29%29%29%29");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SpotifyJwt);

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception($"Can't get this playlist : {errorJson}");
        }

        string json = await response.Content.ReadAsStringAsync();
        Random random = new Random();

        if (string.IsNullOrEmpty(json) || json is null)
        {
            throw new Exception("Playlist is empty");
        }

        SongFromSpotifyPlaylistDTO? songList = JsonSerializer.Deserialize<SongFromSpotifyPlaylistDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (songList?.Tracks.Items is null || songList.Tracks.Items.Count == 0)
        {
            throw new Exception("No songs found in the playlist");
        }

        ItemDto randomSongFromPlaylist = songList.Tracks.Items[random.Next(songList.Tracks.Items.Count)];

        SongDTO songDTO = new()
        {
            Song = randomSongFromPlaylist,
            Genre = GenreName,
            AccessToken = SpotifyJwt,
            EarnedXp = XpGames.GuessGenre,
            OtherGenres = otherGenres
        };

        return songDTO;
    }
}
