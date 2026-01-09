namespace Viberz.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SpotifyId { get; set; } = string.Empty;
        public bool IsGuestGenre { get; set; } = false;
    }
}
