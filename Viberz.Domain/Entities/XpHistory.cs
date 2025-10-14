using System.ComponentModel;
using Viberz.Domain.Enums;

namespace Viberz.Domain.Entities;

public class XpHistory
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int EarnedXp { get; set; }
    public int TotalXp { get; set; }
    public int Level { get; set; }
    public Activies ActivityType { get; set; }
    public NameGenre? Genre { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
