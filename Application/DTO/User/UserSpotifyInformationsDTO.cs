using System.Text.Json.Serialization;
using Viberz.Application.Models;

namespace Viberz.Application.DTO.User;

public class UserSpotifyInformationsDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("userName")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("images")]
    public List<SpotifyImage>? Images { get; set; }

    [JsonPropertyName("type")]
    public string? UserType { get; set; } = "premium";
}