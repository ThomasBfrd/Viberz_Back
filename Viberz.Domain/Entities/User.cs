namespace Viberz.Domain.Entities;

public class User
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Image { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; } = string.Empty;
    public List<string> FavoriteArtists { get; set; } = [];
    public List<string> FavoriteGenres { get; set; } = [];
}