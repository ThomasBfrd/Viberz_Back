namespace LearnGenres.Entities
{
    public class Playlist
    {
        // Id/Name/Created-Updated/Owner/Songs fournis par Spotify
        public int Id { get; set; }
        public string SpotifyPlaylistId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public List<User> LikedByUsers { get; set; } = new List<User>();
    }
}
