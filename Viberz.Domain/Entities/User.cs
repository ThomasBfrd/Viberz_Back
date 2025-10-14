using AutoMapper;
using System.Text.Json.Serialization;
using Viberz.Application.DTO.User;
using Viberz.Application.DTO.Xp;

namespace Viberz.Domain.Entities;

public class User
{

    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("userName")]
    public string? Username { get; set; } = null;
    public string UserType { get; set; } = "premium";
    [JsonPropertyName("favoriteArtists")]
    public List<string>? FavoriteArtists { get; set; } = [];
    [JsonPropertyName("favoriteGenres")]
    public List<string>? FavoriteGenres { get; set; } = [];
}