using System.Text.Json.Serialization;

namespace Viberz.Application.DTO.User;

public class UserUpdateDTO
{
    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("userName")]
    public string? Username { get; set; } = string.Empty;
    [JsonPropertyName("favoriteArtists")]
    public List<string> FavoriteArtists { get; set; } = [];
    [JsonPropertyName("favoriteGenres")]
    public List<string> FavoriteGenres { get; set; } = [];
}
