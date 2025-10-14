namespace Viberz.Domain.Entities;

public class XpGrades
{
    public int Id { get; set; }
    public int Level { get; set; }
    public int MinXp { get; set; }
    public string GradeName { get; set; } = string.Empty;
}
