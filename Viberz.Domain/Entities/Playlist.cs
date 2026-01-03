namespace Viberz.Domain.Entities
{
    public class Playlist
    {
        public int Id { get; set; }
        public string SpotifyPlaylistId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> GenreList { get; set; } = [];
        public int Likes { get; set; } = 0;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
