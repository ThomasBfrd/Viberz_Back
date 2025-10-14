namespace Viberz.Application.DTO.Xp;

public class UserXpDTO
{
    public string UserId { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public int CurrentXp { get; set; } = 0;
    public int XpForPreviousLevel { get; set; } = 0;
    public int XpForNextLevel { get; set; } = 50;
    public string GradeName { get; set; } = "Newbie";
    public bool LevelUp { get; set; } = false;
}
