namespace Viberz.Domain.Entities;

public class LikedPlaylists
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int PlaylistId { get; set; }
}
