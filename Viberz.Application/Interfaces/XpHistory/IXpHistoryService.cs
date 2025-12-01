using Viberz.Application.DTO.Xp;
using Viberz.Domain.Models;

namespace Viberz.Application.Interfaces.XpHistory;

public interface IXpHistoryService
{
    public Task AddXpHistory(string userId, AddXpHistoryGame xpHistory, int newTotal, UserXpInfo beforeUser);
}
