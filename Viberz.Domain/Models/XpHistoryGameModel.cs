using Viberz.Domain.Enums;

namespace Viberz.Domain.Models;

public class XpHistoryGameModel
{
    public int EarnedXp { get; set; }
    public Activies ActivityType { get; set; }
    public NameGenre? Genre { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}