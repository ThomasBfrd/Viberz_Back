using Viberz.Domain.Entities;
using Viberz.Domain.Models;

public interface IXpHistoryRepository
{
    Task<XpHistory> AddHistoryGame(string userId, AddXpHistoryGame historyGame, UserXpInfo user, int newTotal);
}
