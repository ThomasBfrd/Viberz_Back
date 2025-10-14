using AutoMapper;
using Microsoft.OpenApi.MicrosoftExtensions;
using Viberz.Application.DTO.Xp;
using Viberz.Application.Models;
using Viberz.Domain.Entities;
using Viberz.Domain.Enums;
using Viberz.Viberz.Infrastructure.Data;

public class XpHistoryRepository : IXpHistoryRepository
{
    public readonly ApplicationDbContext _context;
    public readonly IMapper _mapper;
    public XpHistoryRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<XpHistory> AddHistoryGame(string userId, AddXpHistoryGame xpHistory, UserXpDTO user, int newTotal)
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
