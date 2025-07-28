using LearnGenres.Entities;

namespace LearnGenres.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public int Xp { get; set; }
        public Genre Genre { get; set; } = new Genre();
    }
}
