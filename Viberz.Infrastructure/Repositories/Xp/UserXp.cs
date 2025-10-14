using MediatR;
using Microsoft.EntityFrameworkCore;
using Viberz.Application.DTO.Xp;
using Viberz.Domain.Entities;
using Viberz.Viberz.Infrastructure.Data;

public class UserXp : IUserXp
{
    private readonly ApplicationDbContext _context;
    public UserXp(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserXpDTO> GetUserXp(string userId)
    {
        List<XpHistory>? userXpHistories = await _context.XpHistories.Where(x => x.UserId == userId).ToListAsync();

        if (userXpHistories is null || userXpHistories.Count == 0)
        {
            return new()
            {
                UserId = userId,
                Level = 1,
                CurrentXp = 0,
                XpForNextLevel = 50,
                GradeName = "Newbie",
                LevelUp = false
            };
        }

        List<XpGrades> grades = await _context.XpGrades.ToListAsync();

        int userLvl = userXpHistories.Count > 0 ? userXpHistories.Max(x => x.Level) : 1;

        int totalXp = userXpHistories.Max(x => x.TotalXp);

        XpGrades? nextGrade = grades.FirstOrDefault(x => x.Level == (userLvl + 1));

        string gradeName = grades.FirstOrDefault(x => x.Level == userLvl)?.GradeName ?? "Newbie";

        int xpForNextLevel = nextGrade != null ? nextGrade.MinXp : 0;

        return new()
        {
            UserId = userId,
            Level = (xpForNextLevel - totalXp <= 0) ? userLvl + 1 : userLvl,
            CurrentXp = totalXp,
            XpForNextLevel = xpForNextLevel,
            GradeName = gradeName
        };
    }
}
