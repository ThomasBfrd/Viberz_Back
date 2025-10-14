namespace Viberz.Domain.Models;

public class UserXpInfo
{
    public string UserId { get; set; } = string.Empty;
    public int Level { get; set; }
    public int CurrentXp { get; set; }
    public int XpForPreviousLevel { get; set; }
    public int XpForNextLevel { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public bool LevelUp { get; set; }
}