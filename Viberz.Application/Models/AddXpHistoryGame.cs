using Viberz.Domain.Enums;

namespace Viberz.Application.Models
{
    public class AddXpHistoryGame
    {
        public int EarnedXp { get; set; }
        public Activies ActivityType { get; set; }
        public NameGenre? Genre { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}
