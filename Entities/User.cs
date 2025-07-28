using System.ComponentModel.DataAnnotations;

namespace LearnGenres.Entities
{
    public class User
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Xp { get; set; } = 0;
        public NameGenre PreferedGenre { get; set; }

        // Relation many-to-many
        public List<User> Followers { get; set; } = new List<User>();
        public List<User> Following { get; set; } = new List<User>();
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
        public List<Playlist> PlaylistsLiked { get; set; } = new List<Playlist>();
    }
}
