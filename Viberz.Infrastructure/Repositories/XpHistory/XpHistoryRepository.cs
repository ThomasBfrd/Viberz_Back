using AutoMapper;
using Viberz.Domain.Entities;
using Viberz.Domain.Models;
using Viberz.Infrastructure.Data;

public class XpHistoryRepository : IXpHistoryRepository
{
    public readonly ApplicationDbContext _context;
    public readonly IMapper _mapper;
    public XpHistoryRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<XpHistory> AddHistoryGame(string userId, AddXpHistoryGame xpHistory, UserXpInfo user, int newTotal)
    {
        XpHistory xp = new()
        {
            UserId = userId,
            EarnedXp = xpHistory.EarnedXp,
            TotalXp = newTotal,
            Level = user.LevelUp ? user.Level + 1 : user.Level,
            ActivityType = xpHistory.ActivityType,
            Genre = xpHistory.Genre,
            CreatedAt = DateTime.UtcNow
        };

        await _context.XpHistories.AddAsync(xp);
        await _context.SaveChangesAsync();

        return xp;
    }
}
