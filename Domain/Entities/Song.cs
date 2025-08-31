namespace Viberz.Domain.Entities
{
    public class Song
    {
        // Id et autres données fournies par Spotify
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public Genre Genre { get; set; } = new Genre();
        public int Duration { get; set; } = 0;
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
    }
}
