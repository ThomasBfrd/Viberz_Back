using Viberz.Application.DTO.Xp;
using Viberz.Application.Interfaces.XpHistory;
using Viberz.Domain.Entities;
using Viberz.Domain.Models;

public class XpHistoryService(IXpHistoryRepository xpHistoryRepository, IUserXp userXpService) : IXpHistoryService
{
    IXpHistoryRepository _xpHistoryRepository = xpHistoryRepository;
    public async Task AddXpHistory(string userId, AddXpHistoryGame xpHistory, int newTotal, UserXpInfo beforeUser)
    {
        XpHistory xp = new()
        {
            UserId = userId,
            EarnedXp = xpHistory.EarnedXp,
            TotalXp = newTotal,
            Level = beforeUser.LevelUp ? beforeUser.Level + 1 : beforeUser.Level,
            ActivityType = xpHistory.ActivityType,
            Genre = xpHistory.Genre,
        };

        await _xpHistoryRepository.Add(xp);
    }
}
