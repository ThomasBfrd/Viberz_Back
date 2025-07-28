using LearnGenres.Entities;
using System.Text.Json.Serialization;

namespace LearnGenres.DTO
{
    public class UserCreationDTO
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public int Xp { get; set; } = 0;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Genre PreferedGenre { get; set; } = new Genre();
    }
}
